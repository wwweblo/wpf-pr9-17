using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;

namespace WpfApp3
{
    internal class WordHelper
    {
        private string _filePath;

        public WordHelper(string filePath)
        {
            if (File.Exists(filePath))
            {
                _filePath = filePath;
            }
            else
            {
                throw new ArgumentException("File not found");
            }
        }

        internal bool Process(Dictionary<string, string> values)
        {
            Word.Application app = null;
            Word.Document doc = null;
            try
            {
                app = new Word.Application();
                app.Visible = true; // Сделаем Word видимым для отладки

                object file = _filePath;
                object missing = Type.Missing;

                doc = app.Documents.Open(ref file, ref missing, ref missing, ref missing);

                foreach (var value in values)
                {
                    Word.Find find = app.Selection.Find;
                    find.Text = value.Key;
                    find.Replacement.Text = value.Value;

                    object wrap = Word.WdFindWrap.wdFindContinue;
                    object replace = Word.WdReplace.wdReplaceAll;

                    find.Execute(
                        FindText: Type.Missing,
                        MatchCase: false,
                        MatchWholeWord: false,
                        MatchWildcards: false,
                        MatchSoundsLike: missing,
                        MatchAllWordForms: false,
                        Forward: true,
                        Wrap: wrap,
                        Format: false,
                        ReplaceWith: missing,
                        Replace: replace
                    );
                }

                string downloadFolder = Path.Combine("C:\\Скачанные");
                if (!Directory.Exists(downloadFolder))
                {
                    Directory.CreateDirectory(downloadFolder);
                }

                string newFileName = Path.Combine(downloadFolder, $"{DateTime.Now:yyyyMMddHHmmss}_Result.docx");
                doc.SaveAs2(newFileName);

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.Message}");
            }
            finally
            {
                doc?.Close();
                app?.Quit();
            }
            return false;
        }
    }
}
