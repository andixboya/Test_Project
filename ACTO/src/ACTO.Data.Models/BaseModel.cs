
namespace ACTO.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;


    public class BaseModel<TKey>
    {
        //for... types of key (string/int)
        [Key]
        public TKey Id { get; set; }
    }
}
