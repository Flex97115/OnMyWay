//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnMyWayCTGV
{
    using System;
    using System.Collections.Generic;
    
    public partial class Chair
    {
        public Chair()
        {
            this.Client = new HashSet<Client>();
        }
    
        public int Id { get; set; }
        public int tableId { get; set; }
    
        public virtual Table Table { get; set; }
        public virtual ICollection<Client> Client { get; set; }
    }
}
