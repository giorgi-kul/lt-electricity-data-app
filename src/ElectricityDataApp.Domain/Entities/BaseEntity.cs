using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDataApp.Domain.Entities
{
    public abstract class BaseEntity
    {
        private DateTime? _createDate;

        public int Id { get; set; }

        public DateTime CreateDate
        {
            get
            {
                return _createDate ?? (_createDate = DateTime.Now).Value;
            }
            set
            {
                _createDate = value;
            }
        }
    }
}
