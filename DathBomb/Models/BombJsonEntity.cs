using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DathBomb.Models
{
    public class BombJsonEntity
    {
        public ReadOnlyCollection<BombTextEntity> Staff { get; set; }

        public BombJsonEntity()
        {
            this.Staff = new ReadOnlyCollection<BombTextEntity>(Array.Empty<BombTextEntity>());
        }

        public BombJsonEntity(IList<BombTextEntity> staff)
        {
            this.Staff = new ReadOnlyCollection<BombTextEntity>(staff);
        }
    }
}
