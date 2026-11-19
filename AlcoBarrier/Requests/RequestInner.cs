using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlcoBarrier
{
    public class RequestInner : IDisposable
    {
        private HttpClient client;

        private readonly string IpAddress;
        private bool _disposed;

        public RequestInner(string ip, string auth, string key)
        {
            IpAddress = ip;
            client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10)
            };

            // ИЗМЕНЕНО (этап C3-fix): HttpHeaders.Add не принимает пустую строку
            // (бросает FormatException). Добавляем заголовок, только если значение непустое.
            // Это защищает от дефолтов из EnsureSettings (auth="", key="") и от ручной
            // очистки настроек пользователем.
            if (!string.IsNullOrEmpty(auth))
                client.DefaultRequestHeaders.Add("Authorization", auth);
            if (!string.IsNullOrEmpty(key))
                client.DefaultRequestHeaders.Add("API-KEY", key);
        }

        async public Task<string> GetSystemInfo(CancellationToken token)
        {
            string MyResult;
            try
            {
                token.ThrowIfCancellationRequested();

                using (HttpResponseMessage response = await client.GetAsync($"http://{IpAddress}/restApi/v2/SystemInfo", token))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MyResult = $"{InnerageXmlParser.GetElementText(responseBody, "SystemInfo")}";
                }
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception e)
            {
                MyResult = $"Message :{e.Message}";
            }
            return MyResult;
        }

        /// <summary>
        /// Загружает сотрудников из Innerage и разбирает ответ.
        /// Возвращает null только при сетевой ошибке/не-2xx: тогда синхронизация
        /// не выполняется и локальная база остаётся прежней (пункт 1).
        /// ИЗМЕНЕНО (рефакторинг XmlHandler): результат разбора типизирован, а сам разбор
        /// выполняется в пуле потоков — раньше XML парсился на UI-контексте.
        /// </summary>
        async public Task<UsersParseResult> GetDictUsers(CancellationToken token)
        {
            string MyResult;
            try
            {
                token.ThrowIfCancellationRequested();

                using (HttpResponseMessage response = await client.GetAsync($"http://{IpAddress}/restApi/v2/User/User?FullObject=True", token))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MyResult = responseBody;
                }
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception e)
            {
                // ИЗМЕНЕНО (пункт 1): при сетевой ошибке или не-2xx ответе возвращаем null,
                // а не строку "Message :...". Пустой/битый ответ не должен приводить
                // к перезаписи локальной базы сотрудников пустой таблицей.
                Logger.Warning($"GetDictUsers failed: {e.Message}");
                return null;
            }

            return await Task.Run(() => InnerageXmlParser.ParseUserCards(MyResult), token);
        }

        /// <summary>
        /// Блокирует (set = true, State=Unused) или разблокирует (set = false, State=Active) карту.
        /// Возвращает true, только если Innerage подтвердил команду (HTTP 2xx без исключения).
        /// ИЗМЕНЕНО (пункт 2): результат возвращается вызывающему коду — от него зависит,
        /// можно ли удалять задачу из events.
        /// </summary>
        async public Task<bool> BlockedUser(bool set, string[] UserData, CancellationToken token)
        {
            string MyResult = string.Empty;
            bool completed = false;

            try
            {
                token.ThrowIfCancellationRequested();

                string command = string.Empty;
                string Active = "Active";

                if (set)
                {
                    Active = "Unused";
                }
                command = $"<User Address=\"{UserData[3]}\">\r\n" +
                          $"<Cards>\r\n" +
                          $"<Card>\r\n<Name>{UserData[1]}</Name>\r\n" +
                          $"<State>{Active}</State>\r\n" +
                          $"<CardType>\r\n<Ref Type=\"CardTemplate\" PartitionID=\"0\" ID=\"{UserData[4]}\" />\r\n" +
                          $"</CardType>\r\n<CardNumber>{UserData[1]}</CardNumber>\r\n<CardNumberNumeric>{UserData[1]}</CardNumberNumeric>\r\n" +
                          $"<IssueNumber>0</IssueNumber>\r\n" +
                          $"<CardData>{UserData[2]}</CardData>\r\n" +
                          $"<ExternalCredentials />\r\n<CloudCredentialType>None</CloudCredentialType>\r\n</Card>\r\n</Cards>\r\n</User>\r\n";

                byte[] messageToBytes = Encoding.UTF8.GetBytes(command);

                using (var content = new ByteArrayContent(messageToBytes))
                using (HttpResponseMessage response = await client.PostAsync($"http://{IpAddress}/restApi/v2/User/AddOrUpdate", content, token))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MyResult = $"{InnerageXmlParser.GetElementText(responseBody, "CommandProgress")}";
                }

                completed = true;
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception e)
            {
                MyResult = $"Message :{e.Message}";
            }

            // ИЗМЕНЕНО (пункт 2): успех пишется как Info, сбой — как Warning.
            // Раньше любой исход логировался как Info, и неудачная блокировка была незаметна.
            if (completed)
                Logger.Info($"BlockedUser set={set}, address={UserData[3]}: {MyResult}");
            else
                Logger.Warning($"BlockedUser set={set}, address={UserData[3]} не выполнено: {MyResult}");

            return completed;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            client.Dispose();
        }
    }
}