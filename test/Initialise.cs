﻿//-----------------------------------------------------------------------------
// Filename: Initialise.cs
//
// Description: Assembly initialiser for SIPSorcery unit tests.
//
// Author(s):
// Aaron Clauson
//
// History:
// 14 Oct 2019	Aaron Clauson	Created (aaron@sipsorcery.com), SIP Sorcery PTY LTD, Dublin, Ireland (www.sipsorcery.com).
//
// License: 
// BSD 3-Clause "New" or "Revised" License, see included LICENSE.md file.
//-----------------------------------------------------------------------------

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Xunit;
using SIPSorcery.SIP;
using SIPSorcery.Sys;

namespace SIPSorcery.UnitTests
{
    //[Trait("Category", "unit")]
    //public class Initialize
    //{
    //    [AssemblyInitialize]
    //    public static void AssemblyInitialize(TestContext context)
    //    {
    //        Console.WriteLine("AssemblyInitialise");
    //        SIPSorcery.Sys.Log.Logger = SimpleConsoleLogger.Instance;
    //    }

    //    [AssemblyCleanup]
    //    public static void AssemblyCleanup()
    //    {
    //        Console.WriteLine("AssemblyCleanup");
    //    }
    //}

    /// <summary>
    /// Getting the Microsoft console logger to work with the mstest framework was unsuccessful. Using this super
    /// simple console logger proved to be a lot easier. Can be revisited if mstest logging ever goes back to 
    /// just working OOTB.
    /// </summary>
    internal class SimpleConsoleLogger : ILogger
    {
        public static SimpleConsoleLogger Instance { get; } = new SimpleConsoleLogger();

        private SimpleConsoleLogger()
        { }

        public IDisposable BeginScope<TState>(TState state)
        {
            return SimpleConsoleLoggerScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss:fff")}] [{Thread.CurrentThread.ManagedThreadId}] [{logLevel}] {formatter(state, exception)}");
        }
    }

    internal class SimpleConsoleLoggerScope : IDisposable
    {
        public static SimpleConsoleLoggerScope Instance { get; } = new SimpleConsoleLoggerScope();

        private SimpleConsoleLoggerScope()
        {}

        public void Dispose()
        {}
    }

    internal class MockSIPChannel : SIPChannel
    {
        public MockSIPChannel(IPEndPoint channelEndPoint)
        {
            ListeningIPAddress = channelEndPoint.Address;
            Port = channelEndPoint.Port;
            SIPProtocol = SIPProtocolsEnum.udp;
            ID = Crypto.GetRandomInt(5).ToString();
        }

        public override void Send(IPEndPoint destinationEndPoint, string message)
        { }

        public override void Send(IPEndPoint destinationEndPoint, byte[] buffer)
        { }

        public override Task<SocketError> SendAsync(IPEndPoint destinationEndPoint, byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public override Task<SocketError> SendAsync(IPEndPoint destinationEndPoint, byte[] buffer, string serverCertificate)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        { }

        public override void Send(IPEndPoint dstEndPoint, byte[] buffer, string serverCN)
        {
            throw new ApplicationException("This Send method is not available in the MockSIPChannel, please use an alternative overload.");
        }

        public override void Dispose()
        { }

        public override Task<SocketError> SendAsync(string connectionID, byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public override bool HasConnection(string connectionID)
        {
            throw new NotImplementedException();
        }

        public override bool HasConnection(IPEndPoint remoteEndPoint)
        {
            throw new NotImplementedException();
        }
    }
}