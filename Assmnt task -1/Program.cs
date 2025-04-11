using System;
using System.Collections.Generic;
using System.IO;

namespace LanghamHotelManagement
{
    class Room
    {
        public int RoomNo { get; set; }
        public bool IsAllocated { get; set; }
    }

    class Customer
    {
        public int CustomerNo { get; set; }
        public string CustomerName { get; set; }
    }

    class RoomAllocation
    {
        public int AllocatedRoomNo { get; set; }
        public Customer AllocatedCustomer { get; set; }
    }

    class Program
    {
        static List<Room> listOfRooms = new List<Room>();
        static List<RoomAllocation> roomAllocations = new List<RoomAllocation>();
        static string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "lhms_studentid.txt");
        static string backupPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "lhms_studentid_backup.txt");

        static void Main(string[] args)
        {
            char ans;
            do
            {
                Console.Clear();
                Console.WriteLine("1. Add Rooms\n2. Display Rooms\n3. Allocate Rooms\n4. De-Allocate Rooms\n5. Display Room Allocation Details\n6. Billing\n7. Save Allocations to File\n8. Load Allocations from File\n0. Backup File\n9. Exit");
                Console.Write("Enter Your Choice: ");

                try
                {
                    int choice = Convert.ToInt32(Console.ReadLine());
                    switch (choice)
                    {
                        case 1: AddRooms(); break;
                        case 2: DisplayRooms(); break;
                        case 3: AllocateRoom(); break;
                        case 4: DeAllocateRoom(); break;
                        case 5: DisplayAllocations(); break;
                        case 6: Console.WriteLine("Billing Feature is Under Construction and will be added soon…!!!"); break;
                        case 7: SaveToFile(); break;
                        case 8: LoadFromFile(); break;
                        case 0: BackupFile(); break;
                        case 9: return;
                        default: Console.WriteLine("Invalid Option!"); break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid number!");
                }

                Console.Write("\nWould You Like To Continue (Y/N)? ");
                ans = Console.ReadKey().KeyChar;
                Console.WriteLine();
            } while (ans == 'y' || ans == 'Y');
        }

        static void AddRooms()
        {
            Console.Write("Enter Room Number: ");
            try
            {
                int roomNo = Convert.ToInt32(Console.ReadLine());
                listOfRooms.Add(new Room { RoomNo = roomNo, IsAllocated = false });
                Console.WriteLine("Room Added Successfully.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid Room Number.");
            }
        }

        static void DisplayRooms()
        {
            foreach (Room room in listOfRooms)
            {
                Console.WriteLine($"Room {room.RoomNo} - {(room.IsAllocated ? "Allocated" : "Available")}");
            }
        }

        static void AllocateRoom()
        {
            try
            {
                Console.Write("Enter Room Number to Allocate: ");
                int roomNo = Convert.ToInt32(Console.ReadLine());
                Room room = listOfRooms.Find(r => r.RoomNo == roomNo);

                if (room == null || room.IsAllocated)
                    throw new InvalidOperationException("Room cannot be allocated.");

                Console.Write("Enter Customer Name: ");
                string name = Console.ReadLine();

                room.IsAllocated = true;
                roomAllocations.Add(new RoomAllocation
                {
                    AllocatedRoomNo = room.RoomNo,
                    AllocatedCustomer = new Customer { CustomerName = name }
                });
                Console.WriteLine("Room allocated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void DeAllocateRoom()
        {
            try
            {
                Console.Write("Enter Room Number to De-Allocate: ");
                int roomNo = Convert.ToInt32(Console.ReadLine());
                Room room = listOfRooms.Find(r => r.RoomNo == roomNo);

                if (room == null || !room.IsAllocated)
                    throw new InvalidOperationException("Room is not currently allocated.");

                room.IsAllocated = false;
                roomAllocations.RemoveAll(r => r.AllocatedRoomNo == roomNo);
                Console.WriteLine("Room de-allocated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void DisplayAllocations()
        {
            foreach (var alloc in roomAllocations)
            {
                Console.WriteLine($"Room {alloc.AllocatedRoomNo} => {alloc.AllocatedCustomer.CustomerName}");
            }
        }

        static void SaveToFile()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine($"--- Allocation Log: {DateTime.Now} ---");
                    foreach (var alloc in roomAllocations)
                    {
                        sw.WriteLine($"Room {alloc.AllocatedRoomNo} - {alloc.AllocatedCustomer.CustomerName}");
                    }
                }
                Console.WriteLine("Data saved to file.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Access denied while writing to file.");
            }
        }

        static void LoadFromFile()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string content = File.ReadAllText(filePath);
                    Console.WriteLine(content);
                }
                else
                {
                    throw new FileNotFoundException("The file was not found.");
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void BackupFile()
        {
            try
            {
                string content = File.ReadAllText(filePath);
                File.AppendAllText(backupPath, content);
                File.WriteAllText(filePath, "");
                Console.WriteLine("Backup complete and original file cleared.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Backup failed: {ex.Message}");
            }
        }
    }
}
