﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VaporStore.Data.Models
{
   public  class Game
    {

        public Game()
        {
            this.GameTags = new HashSet<GameTag>();
            this.Purchases = new HashSet<Purchase>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime ReleaseDate { get; set; }


        [ForeignKey(nameof(Developer))]
        public int DeveloperId { get; set; }
        [Required]
        public virtual Developer Developer { get; set; }

        [ForeignKey(nameof(Genre))]
        public int GenreId { get; set; }

        [Required]
        public virtual Genre Genre { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }

        public virtual ICollection<GameTag> GameTags { get; set; }

    }
}

//•	Id – integer, Primary Key
//•	Name – text (required)
//•	Price – decimal(non - negative, minimum value: 0)(required)
//•	ReleaseDate – Date(required)
//•	DeveloperId – integer, foreign key(required)
//•	Developer – the game’s developer (required)
//•	GenreId – integer, foreign key(required)
//•	Genre – the game’s genre (required)
//•	Purchases - collection of type Purchase
//•	GameTags - collection of type GameTag. Each game must have at least one tag.
