using System;
using System.Collections.Generic;


public class User
{
	public static void Main()
	{
	    PaymentSystem ps = new PaymentSystem();
	    
	    Stadium[] stadiums = new Stadium[2];
	    
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
        
        stadiums[0]= new Stadium(matches1,5,3);
        stadiums[1]= new Stadium(matches2,1,15);
        
        foreach (string i in ps.SeeFutureMatch(stadiums[0]))
        {
            Console.WriteLine(i);
        }
        Console.WriteLine("-------------");
        foreach (string i in ps.SeeFreeSeats(stadiums[0]))
        {
            Console.WriteLine(i);
        }
        Console.WriteLine("-------------");
        foreach (string i in ps.SeeMatchSeats(stadiums[0].matches[1]))
        {
            Console.WriteLine(i);
        }
        Console.WriteLine("-------------");
        
		Console.WriteLine(ps.BuyTicket(stadiums[0],stadiums[0].matches[1],1,1, "fio"));
		
		foreach (string i in ps.SeeMatchSeats(stadiums[0].matches[1]))
        {
            Console.WriteLine(i);
        }
		
		Console.WriteLine(ps.ReturnTicket(stadiums[0],stadiums[0].matches[1],1,1, "fio"));
		Console.WriteLine("-------------");
        
		foreach (string i in ps.SeeMatchSeats(stadiums[0].matches[1]))
        {
            Console.WriteLine(i);
        }
        
        DateTime start= new DateTime(2020,1,15),end = new DateTime(2025,1,15);
		Console.WriteLine(ps.BuySeasonTicket(stadiums[0],start,end,"fio"));
		//Console.WriteLine(ps.BuyTicket(stadiums[0],stadiums[0].matches[1],1,2, "fio"));
		//Console.WriteLine(ps.ReturnTicket(stadiums[0],stadiums[0].matches[1],1,2, "fio"));
		Console.WriteLine(ps.AddToKart(stadiums[0],stadiums[0].matches[1],1,2, "fio"));
		Console.WriteLine(ps.ByInKart());
		Console.WriteLine(ps.ReturnSeasonTicket("fio"));
		Console.WriteLine("-------------");
        foreach (string i in ps.SeeMatchSeats(stadiums[0].matches[1]))
        {
            Console.WriteLine(i);
        }
	}
}


internal class Transaction
{
    internal void maketransaction(string fio,int payment)
    {
        Console.WriteLine($"Transaction: {fio} {payment}"); // для проверки
    }
}

internal class AdapterTransactionPayment
{
    int cost;
    string info;
    Transaction trans;
    internal AdapterTransactionPayment(Transaction trans)
    {
        this.trans=trans;
    }
    internal void MakePaymentTicket(Ticket ticket, bool isreturn=false)
    {
        cost = 1000+ (ticket.match.rows-ticket.seat.row)*50;
        if (isreturn)
            cost=cost*(-1);
        info=ticket.persdata;
        trans.maketransaction(info,cost);
    }
    internal void MakePaymentSeasonTicket(SeasonTicket seasonticket, bool isreturn=false)
    {
        cost = 1000+ (seasonticket.stadium.rows)*50;
        if (isreturn)
            cost=cost*(-1);
        info=seasonticket.persdata;
        trans.maketransaction(info,cost);
    }
}

/*internal class Payment
{
    int cost;
    string info;
    internal MakePaymentTicket(Ticket ticket, bool isreturn=false)
    {
        cost = 1000+ (ticket.match.row-ticket.seat.row)*50;
        if (isreturn)
            cost=cost*(-1);
        info=ticket.persdata;
        Console.WriteLine($"Payment {cost} {info}"); // для проверки
    }
    internal MakePaymentSeasonTicket(SeasTicket seasonticket, bool isreturn=false)
    {
        cost = 1000+ (seasonticket.stadium.rows)*50;
        if (isreturn)
            cost=cost*(-1);
        info=seasonticket.persdata;
        Console.WriteLine($"Payment {cost} {info}"); // для проверки
    }
}*/

internal abstract class Document
{
    internal string persdata;
}

internal class Sale
{
    internal DateTime date = new DateTime();
    internal string personaldata,type;
    internal Sale(string fio, string type)
    {
        personaldata=fio;
        date=DateTime.Now;
        this.type=type;
    }
}

internal class PaymentSystem
{
    internal List<Sale> sales = new List<Sale>(); // rename to cart (sales)
    internal List<Ticket> tickets = new List<Ticket>();
    internal List<SeasonTicket> seasontickets = new List<SeasonTicket>();
    internal Transaction trans = new Transaction();
    internal AdapterTransactionPayment paymentA;
    
    internal List<Ticket> karttickets = new List<Ticket>();
    
    internal PaymentSystem()
    {
        paymentA = new AdapterTransactionPayment(trans);
    }
    
    internal string  AddToKart(Stadium stad,Match match,int placerow,int place, string persdata)
    {
        if(match.seats[placerow][place].state == "taken")
		{
		    return ("Место занято");
		}
		
		match.seats[placerow][place].state = "taken";
		
		Ticket ticket = new Ticket(stad,match,match.seats[placerow][place]);
		ticket.persdata=persdata;
		karttickets.Add(ticket);
		return ("карзина паполненна");
    }
    
    internal string ByInKart()
    {
        foreach (Ticket k in karttickets)
        {
        bool haveseasonticket=false;
		foreach(SeasonTicket i in seasontickets)
		{
		    if (i.persdata==k.persdata & i.end > DateTime.Now & i.stadium == k.stadium)
		        haveseasonticket=true;
		}
		if (!haveseasonticket)
		    paymentA.MakePaymentTicket(k);
		
		tickets.Add(k);
		sales.Add(new Sale(k.persdata,"ticket bought"));
        }
        karttickets.Clear();
        return ("покупка завершена");
    }
    
    internal List<string> SeeFreeSeats(Stadium stad)
    {
        return stad.Information();
    }
    
    internal List<string> SeeMatchSeats(Match match)
    {
        return match.Seats();
    }
    
    internal List<string> SeeFutureMatch(Stadium stad)
    {
        return stad.FutureMatch();
    }
    
	/*internal string BuyTicket(Stadium stad,Match match,int placerow,int place, string persdata)
	{
		if(match.seats[placerow][place].state == "taken")
		{
		    return ("Место занято");
		}
		
		match.seats[placerow][place].state = "taken";
		
		Ticket ticket = new Ticket(stad,match,match.seats[placerow][place]);
		ticket.persdata=persdata;
		
		bool haveseasonticket=false;
		foreach(SeasonTicket i in seasontickets)
		{
		    if (i.persdata==persdata & i.end > DateTime.Now & i.stadium == stad)
		        haveseasonticket=true;
		}
		if (!haveseasonticket)
		    paymentA.MakePaymentTicket(ticket);
		
		tickets.Add(ticket);
		sales.Add(new Sale(ticket.persdata,"ticket bought"));
		return ("успешная покупка билета");
	}*/
	
	internal string ReturnTicket(Stadium stad,Match match,int placerow,int place, string persdata)
	{
		int returnticketindex=-1;
		for (int i=0 ; i< tickets.Count;i++)
		    if (tickets[i].stadium == stad & tickets[i].match == match & tickets[i].seat.row == placerow &  tickets[i].seat.number == place)
		    {
		        returnticketindex = i;
		        break;
		    }
        if (returnticketindex == -1)
            return ("неверные данные");

        DateTime date= new  DateTime(2020,1,15);

		if (tickets[returnticketindex].seat.state == "taken" & tickets[returnticketindex].persdata == persdata & tickets[returnticketindex].match.date > date)//DateTime.Now)
		{
		    
            bool haveseasonticket=false;
	    	foreach(SeasonTicket i in seasontickets)
		    {
		        if (i.persdata==persdata & i.end > DateTime.Now & i.stadium == stad)
		            haveseasonticket=true;
		    }
		    if (!haveseasonticket)
		        paymentA.MakePaymentTicket(tickets[returnticketindex],true);
            sales.Add(new Sale(tickets[returnticketindex].persdata,"ticket returned"));
            tickets[returnticketindex].seat.state = "free";
            tickets.RemoveAt(returnticketindex);
            return ("Успешный возврат билета");
		}
	    else
	        return ("Неверно введены данные или срок возврата прошёл");
	}
	internal string BuySeasonTicket(Stadium stad, DateTime start, DateTime end, string persdata)
	{
		SeasonTicket seasonticket= new SeasonTicket(start,end,stad);
		seasonticket.persdata=persdata;
		paymentA.MakePaymentSeasonTicket(seasonticket);
		seasontickets.Add(seasonticket);
		sales.Add(new Sale(seasonticket.persdata,"seasonticket bought"));
		return("Успешная покупка абонимента");
	}
	internal string ReturnSeasonTicket(string persdata)
	{
		int  returnseasonticketindex=-1;
		for (int i=0;i< seasontickets.Count;i++)
		{
		    if (persdata == seasontickets[i].persdata)
		        returnseasonticketindex=i;
		}
		if (returnseasonticketindex==-1)
		{
		    return ("абонимент не найден");
		}
		DateTime date = new DateTime(2020,1,14);
	    if (seasontickets[returnseasonticketindex].start > date)//DateTime.Now)
	    {
	        bool haveticket=false;
	    	foreach(Ticket i in tickets)
		    {
		        if (i.persdata==persdata)
		            haveticket=true;
		    }
		    if (!haveticket)
		    {
		        paymentA.MakePaymentSeasonTicket(seasontickets[returnseasonticketindex],true);
		    }
		    else
		    {
		        return ("вы уже купили билет");
		    }
	        sales.Add(new Sale(seasontickets[returnseasonticketindex].persdata,"seasonticket returned"));
            seasontickets.RemoveAt(returnseasonticketindex);
            return ("Успешный возврат абонимента");
	    }
	    else
	        return("Возврат абонимента больше невозможен");
	}
}

internal class Stadium
{
    internal List<Match> matches = new List<Match>();
    internal int rows, seatsinrow, capacity; 
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
    internal List<string> Information()
	{
	    List<string> listseats= new List<string>();
	    foreach ( Match m in matches)
	    {
	        foreach (Seat[] s1 in m.seats)
	        {
	            foreach (Seat s in s1)
	            {
	                if (s.state == "free")
	                    listseats.Add($"{m.date} {s.row} {s.number}");
	            }
	        }
	    }
	    return listseats;
	}
	internal List<string> FutureMatch()
	{
	    List<string> listmatch= new List<string>();
	    DateTime datenow = new DateTime(2020,1,15); // дата для проверки
        //datenow = DateTime.Now; //Так должно быть в теории 
	    foreach ( Match m in matches)
	    {
	        if (m.date > datenow)
	            listmatch.Add($"{m.date}");
	    }
	    return listmatch;
	}
}

internal class Match
{
    internal DateTime date;
    internal int rows, seatsinrow;
    internal List<Seat[]> seats = new List<Seat[]>();
    public Match(DateTime date,int rows=1, int seatsinrow=1)
    {
        this.date=date;
        this.rows=rows;
        this.seatsinrow=seatsinrow;
        for (int i=0;i< rows;i++)
        {
            seats.Add(new Seat[seatsinrow]);
            for (int u=0;u< seatsinrow;u++)
                seats[i][u]=new Seat(i,u);
        }
    }
    internal List<string> Seats()
	{
	    List<string> listseats= new List<string>();
	    foreach (Seat[] s1 in seats) 
	   {
	       foreach (Seat s in s1)
	       {
                listseats.Add($"{s.row} {s.number} {s.state}");
	       }
	   }
	return listseats;
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


internal class Ticket : Document
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

internal class SeasonTicket : Document
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
