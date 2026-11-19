using System;
using System.Linq;
using System.Reflection;
using System.Text;
using AlcoBarrier;

/// <summary>
/// Харнесс для проверки парсера Innerage. Компилируется скриптом
/// tests/run-xml-parser-tests.ps1 и запускается из временного каталога.
/// Возвращает 0, если все проверки прошли, иначе количество проваленных.
/// </summary>
internal static class XmlParserHarness
{
    private static int _failed;
    private static MethodInfo _parseCards;
    private static MethodInfo _elementText;

    private static int Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Assembly assembly = typeof(UsersParseResult).Assembly;
        Type parser = assembly.GetType("AlcoBarrier.InnerageXmlParser", true);
        const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        _parseCards = parser.GetMethod("ParseUserCards", flags);
        _elementText = parser.GetMethod("GetElementText", flags);

        if (_parseCards == null || _elementText == null)
        {
            Console.WriteLine("Не найдены методы InnerageXmlParser — харнесс не соответствует сборке.");
            return 99;
        }

        CardsTests();
        ElementTextTests();

        Console.WriteLine();
        if (_failed == 0)
        {
            Console.WriteLine("Все проверки пройдены.");
            return 0;
        }

        Console.WriteLine("Провалено проверок: " + _failed);
        return 1;
    }

    private static void CardsTests()
    {
        const string twoCards =
            "<Users><Rows><User><Name>Иван</Name><Address>U1</Address><Cards>" +
            "<Card><Name>1111</Name><ID>id1</ID><CardData>D1</CardData>" +
            "<CardType><Ref Type=\"CardTemplate\" ID=\"tpl1\" /></CardType></Card>" +
            "<Card><Name>2222</Name></Card></Cards></User></Rows></Users>";

        UsersParseResult two = Parse(twoCards);
        Check("1. две карты дают две записи", two.Cards.Count == 2, Describe(two));
        Check("1a. поля первой карты корректны",
            two.Cards.Count > 0 &&
            two.Cards[0].UserName == "Иван" && two.Cards[0].UserAddress == "U1" &&
            two.Cards[0].CardNumber == "1111" && two.Cards[0].CardId == "id1" &&
            two.Cards[0].CardData == "D1" && two.Cards[0].CardTemplate == "tpl1",
            Describe(two));
        Check("1b. вторая карта не наследует поля первой",
            two.Cards.Count > 1 &&
            two.Cards[1].CardNumber == "2222" && two.Cards[1].CardId == "" &&
            two.Cards[1].CardData == "" && two.Cards[1].CardTemplate == "",
            Describe(two));

        const string withComment =
            "<Users><Rows><!-- служебный комментарий --><User><Name>A</Name><Address>U1</Address>" +
            "<Cards><Card><Name>1111</Name></Card></Cards></User></Rows></Users>";
        UsersParseResult commented = Parse(withComment);
        Check("2. комментарий внутри Rows не создаёт запись", commented.Cards.Count == 1, Describe(commented));

        const string withSpaces =
            "<Users><Rows><User><Name>  Иван </Name><Address>\n U1 \n</Address><Cards>" +
            "<Card><Name> 1111 </Name><ID> id1 </ID></Card></Cards></User></Rows></Users>";
        UsersParseResult spaces = Parse(withSpaces);
        Check("3. пробелы и переводы строк обрезаются",
            spaces.Cards.Count == 1 &&
            spaces.Cards[0].UserName == "Иван" && spaces.Cards[0].UserAddress == "U1" &&
            spaces.Cards[0].CardNumber == "1111" && spaces.Cards[0].CardId == "id1",
            Describe(spaces));

        const string prefixed =
            "<s:Users xmlns:s=\"urn:x\"><s:Rows><s:User><s:Name>A</s:Name><s:Address>U1</s:Address>" +
            "<s:Cards><s:Card><s:Name>1111</s:Name></s:Card></s:Cards></s:User></s:Rows></s:Users>";
        UsersParseResult prefixedResult = Parse(prefixed);
        Check("4. XML с namespace-префиксом разобран",
            prefixedResult.XmlParsed && prefixedResult.StructureFound &&
            prefixedResult.Cards.Count == 1 && prefixedResult.Cards[0].CardNumber == "1111",
            Describe(prefixedResult));

        const string defaultNamespace =
            "<Users xmlns=\"urn:x\"><Rows><User><Name>A</Name><Address>U1</Address>" +
            "<Cards><Card><Name>1111</Name></Card></Cards></User></Rows></Users>";
        UsersParseResult defaultNs = Parse(defaultNamespace);
        Check("5. XML с default namespace разобран",
            defaultNs.StructureFound && defaultNs.Cards.Count == 1, Describe(defaultNs));

        const string refInCard =
            "<Users><Rows><User><Name>A</Name><Address>U1</Address><Cards>" +
            "<Card><Name>1111</Name><Ref Type=\"CardTemplate\" ID=\"tpl1\" /></Card>" +
            "</Cards></User></Rows></Users>";
        UsersParseResult refResult = Parse(refInCard);
        Check("6. Ref прямо в Card даёт шаблон",
            refResult.Cards.Count == 1 && refResult.Cards[0].CardTemplate == "tpl1", Describe(refResult));

        UsersParseResult errorXml = Parse("<Error><Message>Access denied</Message></Error>");
        Check("7. чужой корректный XML: XmlParsed, но структуры нет",
            errorXml.XmlParsed && !errorXml.StructureFound && errorXml.Cards.Count == 0 &&
            !string.IsNullOrEmpty(errorXml.Warning),
            Describe(errorXml));

        UsersParseResult emptyRows = Parse("<Users><Rows /></Users>");
        Check("8. пустой Rows: структура есть, карт нет",
            emptyRows.XmlParsed && emptyRows.StructureFound && emptyRows.Cards.Count == 0 &&
            !string.IsNullOrEmpty(emptyRows.Warning),
            Describe(emptyRows));

        UsersParseResult withoutCards = Parse(
            "<Users><Rows><User><Name>A</Name><Address>U1</Address><Cards /></User></Rows></Users>");
        Check("9. пользователь без карт не даёт записи",
            withoutCards.StructureFound && withoutCards.Cards.Count == 0, Describe(withoutCards));

        const string duplicates =
            "<Users><Rows><User><Name>A</Name><Address>U1</Address><Cards><Card><Name>1111</Name></Card></Cards></User>" +
            "<User><Name>B</Name><Address>U2</Address><Cards><Card><Name>1111</Name></Card></Cards></User></Rows></Users>";
        UsersParseResult duplicatesResult = Parse(duplicates);
        Check("10. дубль номера карты: одна запись + предупреждение",
            duplicatesResult.Cards.Count == 1 && !string.IsNullOrEmpty(duplicatesResult.Warning) &&
            duplicatesResult.Warning.Contains("дубл"),
            Describe(duplicatesResult));

        const string xxe =
            "<!DOCTYPE r [<!ENTITY ext SYSTEM \"file:///C:/Windows/win.ini\">]>" +
            "<Users><Rows><User><Name>&ext;</Name></User></Rows></Users>";
        UsersParseResult xxeResult = Parse(xxe);
        Check("11. XXE отклонён", !xxeResult.XmlParsed, Describe(xxeResult));

        string bomb =
            "<!DOCTYPE lolz [<!ENTITY lol \"lol\">" +
            "<!ENTITY lol1 \"&lol;&lol;&lol;&lol;&lol;&lol;&lol;&lol;&lol;&lol;\">" +
            "<!ENTITY lol2 \"&lol1;&lol1;&lol1;&lol1;&lol1;&lol1;&lol1;&lol1;&lol1;&lol1;\">" +
            "<!ENTITY lol3 \"&lol2;&lol2;&lol2;&lol2;&lol2;&lol2;&lol2;&lol2;&lol2;&lol2;\">" +
            "<!ENTITY lol4 \"&lol3;&lol3;&lol3;&lol3;&lol3;&lol3;&lol3;&lol3;&lol3;&lol3;\">]>" +
            "<Users><Rows><User><Name>&lol4;</Name><Address>U1</Address></User></Rows></Users>";
        UsersParseResult bombResult = Parse(bomb);
        Check("12. entity-бомба отклонена", !bombResult.XmlParsed, Describe(bombResult));

        foreach (string bad in new string[] { "", "   ", "Message :Connection refused", null })
        {
            UsersParseResult badResult = Parse(bad);
            string label = bad == null ? "null" : "\"" + bad + "\"";
            Check("13. невалидный ответ " + label + " => ошибка разбора",
                !badResult.XmlParsed && badResult.Cards.Count == 0, Describe(badResult));
        }
    }

    private static void ElementTextTests()
    {
        Check("14. GetElementText собирает текст вложенных узлов",
            ElementText("<SystemInfo><HostName>ab</HostName><IP>1.2.3.4</IP></SystemInfo>", "SystemInfo") == "ab 1.2.3.4");

        Check("15. GetElementText обрезает пробелы",
            ElementText("<X><A> v </A></X>", "X") == "v");

        Check("16. GetElementText: узел отсутствует => пустая строка",
            ElementText("<X><A>v</A></X>", "SystemInfo") == "");

        Check("17. GetElementText: битый XML => пустая строка",
            ElementText("Message :oops", "SystemInfo") == "");

        const string xxe =
            "<!DOCTYPE r [<!ENTITY ext SYSTEM \"file:///C:/Windows/win.ini\">]>" +
            "<Users><Rows><User><Name>&ext;</Name></User></Rows></Users>";
        Check("18. GetElementText: XXE => пустая строка", ElementText(xxe, "Users") == "");

        Check("19. GetElementText: null => пустая строка", ElementText(null, "SystemInfo") == "");
    }

    private static UsersParseResult Parse(string xml)
    {
        return (UsersParseResult)_parseCards.Invoke(null, new object[] { xml });
    }

    private static string ElementText(string xml, string name)
    {
        return (string)_elementText.Invoke(null, new object[] { xml, name });
    }

    private static void Check(string name, bool ok, string details = "")
    {
        if (ok)
        {
            Console.WriteLine("PASS  " + name);
            return;
        }

        _failed++;
        Console.WriteLine("FAIL  " + name + (details.Length > 0 ? " — " + details : ""));
    }

    private static string Describe(UsersParseResult result)
    {
        if (result == null)
            return "result=null";

        string cards = string.Join("; ", result.Cards.Select(c =>
            $"{c.UserName}|{c.UserAddress}|{c.CardNumber}|{c.CardData}|{c.CardId}|{c.CardTemplate}"));

        return $"XmlParsed={result.XmlParsed}, StructureFound={result.StructureFound}, " +
               $"cards={result.Cards.Count} [{cards}], warning={result.Warning}, error={result.Error}";
    }
}
