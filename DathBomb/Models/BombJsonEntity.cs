using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DathBomb.Models
{
    public class BombJsonEntity
    {
        public ReadOnlyCollection<BombTextEntity> Staff { get; set; }

        public BombJsonEntity()
        {
            Staff = new ReadOnlyCollection<BombTextEntity>(Array.Empty<BombTextEntity>());
        }

        public BombJsonEntity(IList<BombTextEntity> staff)
        {
            Staff = new ReadOnlyCollection<BombTextEntity>(staff);
        }
    }
}
