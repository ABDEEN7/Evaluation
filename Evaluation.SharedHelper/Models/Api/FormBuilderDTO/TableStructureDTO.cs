using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.FormBuilderDTO
{
    public class TableStructureDTO
    {
        public string name { get; set; }
        public string nameAr { get; set; }
        public string nameEn { get; set; }
        public int order { get; set; }
        public FieldTable[] fields { get; set; }
        public object licenseFields { get; set; }
        public object dropDownValues { get; set; }
    }

    public class FieldTable
    {
        public string fieldId { get; set; }
        public string value { get; set; }
        public string fieldName { get; set; }
        public string fieldNameAr { get; set; }
        public string fieldNameEn { get; set; }
        public string fieldTooltip { get; set; }
        public int row { get; set; }
        public int column { get; set; }
        public string type { get; set; }
        public string formGroupId { get; set; }
        public string formGroupName { get; set; }
        public string formGroupNameAr { get; set; }
        public string formGroupNameEn { get; set; }
        public int formGroupOrderNo { get; set; }
        public Attribute[] attributes { get; set; }
        public string dropDownTypeId { get; set; }
    }

    public class Attribute
    {
        public string AttributeId { get; set; }
        public string value { get; set; }
        public string messageAr { get; set; }
        public string messageEn { get; set; }
        public string message { get; set; }
    }

}
