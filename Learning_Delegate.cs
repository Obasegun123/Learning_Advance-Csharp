using System;

namespace DelegatesSolution
{
    // declare the delegate type used to calculate the fees
    public delegate void ShippingFeesDelegate(decimal thePrice, ref decimal fee);

    // This is a base class that is used as a foundation 
    // for each of the destination zones
    abstract class ShippingDestination
    {
        public bool m_isHighRisk;
        public virtual void calcFees(decimal price, ref decimal fee) { }

        // This static method returns an actual ShippingDestination object
        // given the name of the destination, or null if none exists
        public static ShippingDestination getDestinationInfo(string dest)
        {
            if (dest.Equals("zone1"))
            {
                return new Dest_Zone1();
            }
            if (dest.Equals("zone2"))
            {
                return new Dest_Zone2();
            }
            if (dest.Equals("zone3"))
            {
                return new Dest_Zone3();
            }
            if (dest.Equals("zone4"))
            {
                return new Dest_Zone4();
            }
            return null;
        }
    }

    // Now we define implementation classes for each of the real shipping
    // destinations. We can add as many as we like as the need arises

    class Dest_Zone1 : ShippingDestination
    {
        public Dest_Zone1()
        {
            this.m_isHighRisk = false;
        }
        public override void calcFees(decimal price, ref decimal fee)
        {
            fee = price * 0.25m;
        }
    }

    class Dest_Zone2 : ShippingDestination
    {
        public Dest_Zone2()
        {
            this.m_isHighRisk = true;
        }
        public override void calcFees(decimal price, ref decimal fee)
        {
            fee = price * 0.12m;
        }
    }

    class Dest_Zone3 : ShippingDestination
    {
        public Dest_Zone3()
        {
            this.m_isHighRisk = false;
        }
        public override void calcFees(decimal price, ref decimal fee)
        {
            fee = price * 0.08m;
        }
    }

    class Dest_Zone4 : ShippingDestination
    {
        public Dest_Zone4()
        {
            this.m_isHighRisk = true;
        }
        public override void calcFees(decimal price, ref decimal fee)
        {
            fee = price * 0.04m;
        }
    }
}

//Implementation

using System;

namespace DelegatesSolution
{
    class Program
    {
        static void Main(string[] args)
        {
            ShippingFeesDelegate theDel;
            ShippingDestination theDest;

            string theZone;
            do
            {
                // get the destination zone
                Console.WriteLine("What is the destination zone?");
                theZone = Console.ReadLine();

                // if the user wrote "exit" then terminate the program,
                // otherwise continue 
                if (!theZone.Equals("exit"))
                {
                    // given the zone, retrieve the associated shipping info
                    theDest = ShippingDestination.getDestinationInfo(theZone);

                    // if there's no associated info, then the user entered
                    // an invalid zone, otherwise continue
                    if (theDest != null)
                    {
                        // ask for the price and convert the string to a decimal number
                        Console.WriteLine("What is the item price?");
                        string thePriceStr = Console.ReadLine();
                        decimal itemPrice = decimal.Parse(thePriceStr);

                        // Each ShippingDestination object has a function called calcFees,
                        // use that as the delegate for calculating the fee
                        theDel = theDest.calcFees;

                        // For high-risk zones, we tack on the delegate that adds more
                        if (theDest.m_isHighRisk)
                        {
                            theDel += delegate (decimal thePrice, ref decimal itemFee)
                            {
                                itemFee += 25.0m;
                            };
                        }

                        // now all that is left to do is call the delegate and output
                        // the shipping fee to the Console
                        decimal theFee = 0.0m;
                        theDel(itemPrice, ref theFee);
                        Console.WriteLine("The shipping fees are: {0}", theFee);
                    }
                    else
                    {
                        Console.WriteLine("Hmm, you seem to have entered an uknown destination. Try again or 'exit'");
                    }
                }
            } while (theZone != "exit");
        }
    }
}

