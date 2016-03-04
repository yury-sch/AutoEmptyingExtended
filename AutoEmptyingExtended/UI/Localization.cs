using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using ColossalFramework.Globalization;

namespace AutoEmptyingExtended.UI
{
    public class Localization
    {
        private static readonly string DEFAULT_TRANSLATION_PREFIX = "lang";

        private static Localization _instance;
        private readonly string _assemblyPath;
        private static Dictionary<string, string> _translations;

        private Localization()
        {
            _assemblyPath = $"{Assembly.GetExecutingAssembly().GetName().Name}.Resources.";
            _translations = LoadTranslations();
        }

        public static Localization Instance => _instance ?? (_instance = new Localization());
        
        private string GetTranslatedFileName(string filenamePrefix)
        {
            var language = LocaleManager.instance.language;
            switch (language)
            {
                case "jaex":
                    language = "ja";
                    break;
            }

            var filenameBuilder = new StringBuilder(filenamePrefix);
            if (language != null)
            {
                filenameBuilder.Append("_");
                filenameBuilder.Append(language.Trim().ToLower());
            }
            filenameBuilder.Append(".xml");

            var translatedFilename = filenameBuilder.ToString();

            var assembly = Assembly.GetExecutingAssembly();
            if (assembly.GetManifestResourceNames().Contains(_assemblyPath + translatedFilename))
            {
                return translatedFilename;
            }

            if (language != null && !"en".Equals(language))
                Logger.LogWarning($"Translated file {translatedFilename} not found!");
            return filenamePrefix + "_en.xml";
        }

        private Dictionary<string, string> LoadTranslations()
        {
            var translations = new Dictionary<string, string>();
            try
            {
                var filename = _assemblyPath + GetTranslatedFileName(DEFAULT_TRANSLATION_PREFIX);
                string xml;
                using (var rs = Assembly.GetExecutingAssembly().GetManifestResourceStream(filename))
                {
                    using (var sr = new StreamReader(rs))
                    {
                        xml = sr.ReadToEnd();
                    }
                }

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                var nodes = xmlDoc.SelectNodes(@"//Language/LocaleResource");

                foreach (XmlNode node in nodes)
                {
                    var name = node.Attributes["Name"].InnerText.Trim();
                    var value = "";
                    var valueNode = node.SelectSingleNode("Value");
                    if (valueNode != null)
                        value = valueNode.InnerText;

                    translations.Add(name, value);
                }
            }
            catch (Exception e)
            {
                Logger.LogError($"Error while loading translations: {e}");
            }
            return translations;
        }

        public string GetString(string key)
        {
            string ret = null;
            try
            {
                _translations.TryGetValue(key, out ret);
            }
            catch (Exception e)
            {
                Logger.LogError($"Error fetching the key {key} from the translation dictionary: {e}");
                return key;
            }
            if (ret == null)
                return key;
            return ret;
        }

        internal static int GetMenuWidth()
        {
            switch (LocaleManager.instance.language)
            {
                case null:
                case "en":
                case "de":
                default:
                    return 210;
                case "ru":
                case "pl":
                    return 250;
                case "pr":
                case "fr":
                    return 230;
            }
        }
    }

    public static class LocalizationExtensions
    {
        public static string Translate(this String key)
        {
            var localization = Localization.Instance;
            return localization.GetString(key);

        }
    }
}
