using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Word;
namespace Aviadispetcher
{
    class ConvertDataInDoc
    {
        Application wordApp = null;
        Document wordDoc = null;
        string filePath = "";
        public void ConvertFlightListInDoc(List<Flight> selXList, List<Flight> selXYList)
        {
            try
            {
                filePath = Environment.CurrentDirectory.ToString();
                wordApp = new Application();
                wordDoc = wordApp.Documents.Add(filePath + "\\Шаблон_Пошуку_рейсів.dotm");
            }
            catch(Exception ex)
            {
                MainWindow.ErrorShow(ex, "Помістіть файл Шаблон_Пошуку_рейсів.dot"
                    + char.ConvertFromUtf32(13) + "у каталог із exe-файлом програми і повторіть збереження",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            ReplaceText(MainWindow.selectedCity, "[X]");
            ReplaceText(selXList, 1);
            ReplaceText(MainWindow.timeFlight.ToString(@"hh\:mm"), "[Y]");
            ReplaceText(selXYList, 2);
            try
            {
                wordDoc.Save();
            } catch (Exception ex)
            {
                MainWindow.ErrorShow(ex, "Помилка збереження відібраних даних",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);

            }
        }

        private void ReplaceText(string textToReplace, string replacedText)
        {
            Object missing = Type.Missing;
            Microsoft.Office.Interop.Word.Range selText;
            selText = wordDoc.Range(wordDoc.Content.Start, wordDoc.Content.End);
            Microsoft.Office.Interop.Word.Find find = wordApp.Selection.Find;
            find.Text = replacedText;
            find.Replacement.Text = textToReplace;
            Object wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;
            Object replace = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;


            find.Execute(FindText: Type.Missing,
                MatchCase: false,
                MatchWholeWord: false,
                MatchWildcards: false,
                MatchSoundsLike: missing,
                MatchAllWordForms: false,
                Forward: true,
                Wrap: wrap,
                Format: false,
                ReplaceWith: missing, Replace: replace);
        }
        private void ReplaceText(List<Flight> selectedList, int numTable)
        {
            for (int i = 0; i < selectedList.Count; i++)
            {
                wordDoc.Tables[numTable].Rows.Add();
                wordDoc.Tables[numTable].Cell(2 + i, 1).Range.Text =
                    selectedList[i].number;
                wordDoc.Tables[numTable].Cell(2 + i, 2).Range.Text =
                    selectedList[i].depature_time.ToString();
                if (numTable == 2)
                {
                    wordDoc.Tables[numTable].Cell(2 + i, 3).Range.Text =
                        selectedList[i].free_seats.ToString();
                }
            }
        }
        ~ConvertDataInDoc()
        {
            if (wordDoc != null)
            {
                wordDoc.Close(WdSaveOptions.wdPromptToSaveChanges);
            }
            if (wordDoc != null)
            {
                wordApp.Quit(WdSaveOptions.wdPromptToSaveChanges);
            }
        }
    }
}
