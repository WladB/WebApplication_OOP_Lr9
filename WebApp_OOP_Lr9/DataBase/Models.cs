using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp_OOP_Lr9.DataBase
{
    public class NewBuilding
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва будівлі є обов'язковою")]
        [StringLength(100, ErrorMessage = "Назва не може перевищувати 100 символів")]
        public string Caption { get; set; }

        [Required(ErrorMessage = "Адреса є обов'язковою")]
        [StringLength(200, ErrorMessage = "Адреса не може перевищувати 200 символів")]
        public string Address { get; set; }
    }

    public class Property
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Кількість кімнат є обов'язковою")]
        [Range(1, 5, ErrorMessage = "Кількість кімнат має бути від 1 до 5")]
        public int CountRooms { get; set; }

        [Required(ErrorMessage = "Id будівлі є обов'язковим")]
        public int NewBuildingId { get; set; }

        [Required(ErrorMessage = "Площа є обов'язковою")]
        [Range(1, 1000, ErrorMessage = "Площа має бути від 1 до 1000")]
        public float Area { get; set; }

        [Required(ErrorMessage = "Поверх є обов'язковим")]
        [Range(1, 100, ErrorMessage = "Поверх має бути від 1 до 100")]
        public byte Floor { get; set; }
    }

    public class FinancingOption
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва є обов'язковою")]
        [StringLength(100, ErrorMessage = "Назва не може перевищувати 100 символів")]
        public string Caption { get; set; }

        [Required(ErrorMessage = "Опис є обов'язковим")]
        [StringLength(500, ErrorMessage = "Опис не може перевищувати 500 символів")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Id будівлі є обов'язковим")]
        public int NewBuildingId { get; set; }
    }
}
