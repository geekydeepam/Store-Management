using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store_Management.Common
{
    public class ModuleMst
    {
        [Key]
        public int pk_moduleID { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDesc { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string  AreaName { get; set; }
        public string ImageUrl { get; set; }
        public int IsActive { get; set; }   

    }
}