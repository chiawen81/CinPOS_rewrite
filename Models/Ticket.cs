using System.ComponentModel.DataAnnotations;
namespace CinPOS_rewrite.Models;

public class Ticket
{
    [Key]                                           // 標註 [Key] 代表此欄位為主鍵
    // <主鍵> 票券編號
    public string TicketId { get; set; } = null!;

    // <外來鍵> 訂單邊號
    public string OrderId { get; set; } = null!;
    //public Order Order { get; set; }

    // <外來鍵> 電影編號
    public string MovieId { get; set; } = null!;
    //public Movie Movie { get; set; };

    // <外來鍵> 座位編號
    public string SeatId { get; set; } = null!;
    //public Seat Seat { get; set; };

    // <外來鍵> 場次表
    public string ScheduleId { get; set; } = null!;
    //public TimeTable TimeTable { get; set; };

    // <外來鍵> 票券類型
    public int TicketTypeId { get; set; }
    public TicketType TicketType { get; set; } = null!;



    // 票價
    public decimal Price { get; set; }

    // 付款方法
    public int PaymentMethod { get; set; }

    // 是否退票
    public bool IsRefund { get; set; }

    // 推款方法
    public int RefundMethod { get; set; }

    // 建立時間
    public DateTime? CreatedAt { get; set; }

    // 更新時間
    public DateTime? UpdatedAt { get; set; }
    
}