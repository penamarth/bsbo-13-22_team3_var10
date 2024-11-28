using System;
using System.Collections.Generic;

public class User
{
	public static void Main() // здесь заполнение данными и вызовы функций
	{
	    DateTime[] date = new DateTime[6]; 
	    date[0] = new DateTime(2020,1,10);
	    date[1] = new DateTime(2020,2,10);
	    date[2] = new DateTime(2020,3,10);
	    date[3] = new DateTime(2021,1,10);
	    date[4] = new DateTime(2021,2,10);
	    date[5] = new DateTime(2021,3,10);
	    
	    List<Match> matches1 = new List<Match>(3);
        matches1.Add(new Match(date[0]));
        matches1.Add(new Match(date[1]));
        matches1.Add(new Match(date[2]));
        List<Match> matches2 = new List<Match>(3);
        matches2.Add(new Match(date[3]));
        matches2.Add(new Match(date[4]));
        matches2.Add(new Match(date[5]));
        
        Document.stadiums[0]= new Stadium(matches1,2,10);
        Document.stadiums[1]= new Stadium(matches2,1,15);
        
		PaymentSystem.BuyTicket();
		PaymentSystem.BuyTicket();
		PaymentSystem.ReturnTicket();
		PaymentSystem.BuySeasonTicket();
		PaymentSystem.ReturnSeasonTicket();
	}
}

internal static class Document
{
    internal static List<string> personaldataticket = new List<string>();
    internal static List<string> personaldataseasonticket = new List<string>();
    internal static List<Ticket> tickets = new List<Ticket>();
    internal static List<SeasonTicket> seasontickets = new List<SeasonTicket>();
    internal static Stadium[] stadiums = new Stadium[2]; 
}

public static class Sales 
{
    internal static List<Ticket> ticketsbought = new List<Ticket>();
    internal static List<Ticket> ticketsreturned = new List<Ticket>();
    internal static List<SeasonTicket> seasontickets = new List<SeasonTicket>();
    internal static List<SeasonTicket> seasonticketsreturned = new List<SeasonTicket>();
}

internal class PaymentSystem
{
    //public static Document document = new Document();
	internal static void BuyTicket()
	{
	    int stadiumnumber=1; // вводится пользователем
	    Document.stadiums[stadiumnumber].FutureMatch();
	    int matchnumber=2; // вводится пользователем
		Document.stadiums[stadiumnumber].matches[matchnumber].PrintSeats();
		int placerow = 0 , place = 1; // вводится пользователем
		// где-то здесь оплата (с учётом абонимента)
		Document.personaldataticket.Add("Personal data buy ticket");// вводится пользователем
		if(Document.stadiums[stadiumnumber].matches[matchnumber].seats[placerow][place].state == "taken")
		{
		    Console.WriteLine("Место занято");
		    return;
		}
		
		Document.stadiums[stadiumnumber].matches[matchnumber].seats[placerow][place].state = "taken";
		
		Ticket ticket = new Ticket(Document.stadiums[stadiumnumber],Document.stadiums[stadiumnumber].matches[matchnumber],Document.stadiums[stadiumnumber].matches[matchnumber].seats[placerow][place]);
		
		Document.tickets.Add(ticket);
		Sales.ticketsbought.Add(ticket);
		Console.WriteLine("успешная покупка билета");
		
	}
	
	internal static void ReturnTicket()
	{
	    int stadiumnumber=1; // вводится пользователем
	    int matchnumber=2; // вводится пользователем
		int placerow = 0 , place = 1; // вводится пользователем
		int returnticketindex=-1;
		for (int i=0 ; i< Document.tickets.Count;i++)
		    if (Document.tickets[i].stadium == Document.stadiums[stadiumnumber] & Document.tickets[i].match == Document.stadiums[stadiumnumber].matches[matchnumber] & Document.tickets[i].seat.row == placerow &  Document.tickets[i].seat.number == place)
		    
		        returnticketindex = i;
		    else
		    {
		        Console.WriteLine("Неверные данные");
		        return;
		    }
		//int returnticketindex = Document.tickets.IndexOf(returnticket);
		string returndata="Personal data buy ticket"; // вводится пользователем
		if (returnticketindex>-1 & Document.tickets[returnticketindex].seat.state == "taken" & Document.personaldataticket[returnticketindex] == returndata & Document.tickets[returnticketindex].match.date > DateTime.Now) // можно сделать отдельные условия, для выводов ошибок по каждому из них
		{
            // где-то здесь возвращение денег (с учётом абонимента)
            Document.tickets[returnticketindex].seat.state = "free";
            Sales.ticketsreturned.Add(Document.tickets[returnticketindex]);
            Document.tickets.RemoveAt(returnticketindex);
            Document.personaldataticket.RemoveAt(returnticketindex);
            Console.WriteLine("Успешный возврат билета");
		}
	    else
	        Console.WriteLine("Неверно введены данные или срок возврата прошёл");
	}
	internal static void BuySeasonTicket()
	{
		int stadiumnumber=1; // вводится пользователем
		DateTime start = new DateTime(2020,10,4),end=new DateTime(2025,5,1); // вводится пользователем
		Document.personaldataseasonticket.Add("Personal data season ticket");// вводится пользователем
		
		// где-то здесь оплата
		
		SeasonTicket seasonticket= new SeasonTicket(start,end,Document.stadiums[stadiumnumber]);
		Document.seasontickets.Add(seasonticket);
		Sales.seasontickets.Add(seasonticket);
		Console.WriteLine("Успешная покупка абонимента");
	}
	internal static void ReturnSeasonTicket()
	{
		int  returnseasonticketindex; // вводится пользователем
	    returnseasonticketindex = Document.personaldataseasonticket.IndexOf("Personal data season ticket");// вводится пользователем
	    DateTime datenow = new DateTime();
	    datenow=DateTime.Now;
	    if (Document.seasontickets[returnseasonticketindex].start > datenow)
	    {
	        // возврат средств
            Sales.seasonticketsreturned.Add(Document.seasontickets[returnseasonticketindex]);
            Document.seasontickets.RemoveAt(returnseasonticketindex);
            Document.personaldataticket.RemoveAt(returnseasonticketindex);
            Console.WriteLine("Успешный возврат абонимента");
	    }
	    else
	        Console.WriteLine("Возврат абонимента больше невозможен");
	}
}

internal class Stadium
{
    internal List<Match> matches = new List<Match>();
    int rows, seatsinrow, capacity; 
    public Stadium(List<Match> newmatches,int rows = 2, int seatsinrow = 10)
    {
        this.rows=rows;
        this.seatsinrow=seatsinrow;
        for (int i=0 ; i < newmatches.Count;i++)
        {
            matches.Add(new Match(newmatches[i].date,rows,seatsinrow)); 
        }
        capacity = rows*seatsinrow;
    }
    internal void PrintInformation()
	{
	    Console.WriteLine("Свободные места:");
	    foreach ( Match m in matches)
	    {
	        foreach (Seat[] s1 in m.seats)
	        {
	            foreach (Seat s in s1)
	            {
	                if (s.state == "free")
	                    Console.WriteLine($"{m.date} {s.row} {s.number}");
	            }
	        }
	    }
	}
	internal void FutureMatch()
	{
	    DateTime datenow = new DateTime(2020,1,15); // дата для проверки
        //datenow = DateTime.Now; //Так должно быть в теории 
	    foreach ( Match m in matches)
	    {
	        if (m.date > datenow)
	            Console.WriteLine(m.date);
	    }
	}
}

internal class Match
{
    internal DateTime date;
    internal int rows, seatsinrow; // новые переменные для заполнения билетов (ряд и количество мест в ряду — номер)
    internal List<Seat[]> seats = new List<Seat[]>();
    public Match(DateTime date,int rows=1, int seatsinrow=1)
    {
        this.date=date;
        this.rows=rows;
        this.seatsinrow=seatsinrow;
        for (int i=0;i< rows;i++) //заполнение билетов
        {
            seats.Add(new Seat[seatsinrow]);
            for (int u=0;u< seatsinrow;u++)
                seats[i][u]=new Seat(i,u);
        }
    }
    internal void PrintSeats()
	{
	    Console.WriteLine("Места:"); 
	    foreach (Seat[] s1 in seats) 
	   {
	       foreach (Seat s in s1)
                Console.WriteLine($"{s.row} {s.number} {s.state}");
	   }
	}
}

internal class Seat
{
    internal int row;
    internal int number;
    public Seat(int row,int number)
    {
        this.row=row;
        this.number=number;
    }
    internal string state="free";
}


internal class Ticket
{
    internal Stadium stadium;
    internal Match match;
    internal Seat seat;
    public Ticket(Stadium stadium,Match match,Seat seat)
    {
        this.stadium = stadium;
        this.match = match;
        this.seat = seat;
    }
}

internal class SeasonTicket
{
    internal DateTime start;
    internal DateTime end;
    internal Stadium stadium;
    public SeasonTicket(DateTime start,DateTime end,Stadium stadium)
    {
        this.start = start;
        this.end = end;
        this.stadium =stadium;
    }
}
