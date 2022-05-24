using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.CommandDesignPattern.Commands
{
    public class FileCreateInvoker
    {
        private ITableActionCommand _tableActionCommand; //readonly vermiyoruz burda çünkü >>> method''un içerisinde set ediyoruz.
        //tüm command'leri "ITableActionCommand" üzerinden yaptığımız için alıyoruz.
        private List<ITableActionCommand> tableActionCommands = new List<ITableActionCommand>(); //"pdf ve excel oluştur" butonu veya "checkbox"tan
                                                                                                 //seçili alanları da oluşturmamız için gerekli olan kod.
        public void SetCommand(ITableActionCommand tableActionCommand)
        {
            _tableActionCommand = tableActionCommand;
        }
        public void AddCommand(ITableActionCommand tableActionCommand)
        {
            tableActionCommands.Add(tableActionCommand);
        }
        public IActionResult CreateFile()
        {
            return _tableActionCommand.Execute();
        }
        public List<IActionResult> CreateFiles()
        {
            return tableActionCommands.Select(x => x.Execute()).ToList();
        }
    }
}
