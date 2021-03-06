﻿using System;

namespace Thread.Pool
{
    class Program
    {
        static bool tickRunning;
        static bool TickRunning{
            get{
                return tickRunning;
            }
            set{
                tickRunning = value;
            }
        }
        static void DoThread(){
            Console.WriteLine("Hello from Thread!!");
            System.Threading.Thread.Sleep(5000);
            Console.WriteLine($"Finalizando ejecución de Thread!!");
            Console.WriteLine($"Valor de prioridad de Hilo #{System.Threading.Thread.CurrentThread.ManagedThreadId} actual (DoThread): " +
                $"{System.Threading.Thread.CurrentThread.Priority}");
            Console.WriteLine("Es proceso en segundo plano? " + (System.Threading.Thread.CurrentThread.IsBackground? "Si": "No"));
        }

        static void CreateThreadLambdaExpression(){
            /*/System.Threading.Thread Thread = new System.Threading.Thread(
                delegate (){
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine("Hello from Thread!!");
                }
            );*/

            System.Threading.Thread Thread = new System.Threading.Thread(
                () => {
                    //System.Threading.Thread.Sleep(1000);
                    Console.WriteLine("Hello from Thread!!");
                }
            );

            Thread.Start();
        }

        static void CreateFirstThread(){
System.Threading.Thread MyFirstThread = new System.Threading.Thread(DoThread);
            //System.Threading.Thread MyFirstThread = new System.Threading.Thread(
            //    new System.Threading.ThreadStart(DoThread));
            MyFirstThread.Start();
        }

        static void CreateATask(){
            System.Threading.Tasks.Task Task = 
                System.Threading.Tasks.Task.Run(delegate{
                    Console.WriteLine("Iniciando ejecución de tarea!!!");
                    System.Threading.Thread.Sleep(5000);
                    Console.WriteLine("FInalizando ejecución de tarea!!!");
                });
        }

        static void DowWork(object obj){
            Console.WriteLine("Working on {0}", obj);
            System.Threading.Thread.Sleep(1000);
        }

        static void CreateATaskPTS(object value){
            System.Threading.Thread Thread = 
                new System.Threading.Thread(
                    new System.Threading.ParameterizedThreadStart(DowWork)
                    );
            
            Thread.Start(value);
        }

        static void CreateATaskPTSLE(object value){
            /*System.Threading.Thread Thread = 
                new System.Threading.Thread(
                    (obj)=>{DowWork(obj);}
                );*/

            System.Threading.Thread Thread = 
                new System.Threading.Thread(
                    delegate (object obj) {DowWork(obj);}
                );
            
            Thread.Start(value);
        }

        static void AbortAThreadWithSharedVariable(object _ms){
            System.Threading.Thread THread = new System.Threading.Thread(
                ms => {
                    int Ms;
                    if(int.TryParse(ms.ToString(), out Ms)){
                        Console.WriteLine($"Hilo que modifica variable compartida (true) {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                        TickRunning = true;
                    while (TickRunning)
                    {
                        Console.WriteLine("Tick...");
                        System.Threading.Thread.Sleep(Ms);
                    }
                    }else{
                        Console.WriteLine("Valor incorrecto!!");
                    }
                }
            );

            THread.Start(_ms);

            Console.WriteLine("Press any key to stop the clock...");
            Console.ReadKey();
            Console.WriteLine($"Hilo que modifica variable compartida (false) {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            TickRunning = false;
        }

        static void AbortAThread(object _ms){
            System.Threading.Thread THread = new System.Threading.Thread(
                ms => {
                    int Ms;
                    if(int.TryParse(ms.ToString(), out Ms)){
                    while (true)
                    {
                        Console.WriteLine("Tick...");
                        System.Threading.Thread.Sleep(Ms);
                    }
                    }else{
                        Console.WriteLine("Valor incorrecto!!");
                    }
                }
            );

            THread.Start(_ms);

            Console.WriteLine("Press any key to stop the clock...");
            Console.ReadKey();
            THread.Abort();
        }
        static void Main(string[] args)
        {
            //CreateATask();
            //CreateFirstThread();
            //CreateThreadLambdaExpression();
            //System.Threading.Thread.Sleep(1500);
            //CreateATaskPTS(2509);

            //CreateATaskPTSLE(2509);
            AbortAThread(1000);
            //AbortAThreadWithSharedVariable(500);
            Console.WriteLine("Finalizando ejecución de método MAIN");
            //Console.WriteLine($"Valor de prioridad de Hilo #{System.Threading.Thread.CurrentThread.ManagedThreadId} actual (main): " +
            //    $"{System.Threading.Thread.CurrentThread.Priority}");
            //Console.WriteLine("Es proceso en segundo plano? " + (System.Threading.Thread.CurrentThread.IsBackground? "Si": "No"));
            Console.WriteLine("Press any key to end.");
        }
    }
}