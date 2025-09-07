// Copyright Epic Games, Inc. All Rights Reserved.

using System;

namespace Epic.OnlineServices.P2P
{
	/// <summary>
	/// Structure containing information about who would like to receive a packet, and how much data can be stored safely.
	/// </summary>
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	public struct ReceivePacketOptions
	{
		internal byte[] m_RequestedChannel;
		/// <summary>
		/// The Product User ID of the user who is receiving the packet
		/// </summary>
		public ProductUserId LocalUserId { get; set; }

		/// <summary>
		/// The maximum amount of data in bytes that can be safely copied to OutData in the function call
		/// </summary>
		public uint MaxDataSizeBytes { get; set; }

		/// <summary>
		/// An optional channel to request the data for. If <see langword="null" />, we're retrieving the next packet on any channel
		/// </summary>
		public byte? RequestedChannel
		{
			get
			{
				if (m_RequestedChannel == null)
				{
					return null;
				}

				return m_RequestedChannel[0];
			}
			set
			{
				if (value != null)
				{
					if (m_RequestedChannel == null)
					{
						m_RequestedChannel = new byte[1];
					}
					m_RequestedChannel[0] = value.Value;
				}
				else
				{
					m_RequestedChannel = null;
				}
			}
		}
	}

	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	internal struct ReceivePacketOptionsInternal : IDisposable
	{
		private int m_ApiVersion;
		private IntPtr m_LocalUserId;
		private uint m_MaxDataSizeBytes;
		public IntPtr m_RequestedChannel;

		public ReceivePacketOptionsInternal(ref ReceivePacketOptions other)
		{
			m_ApiVersion = P2PInterface.RECEIVEPACKET_API_LATEST;
			m_RequestedChannel = IntPtr.Zero;
			if (other.RequestedChannel.HasValue)
			{
				m_RequestedChannel = Helper.AddPinnedBuffer(other.m_RequestedChannel);
			}
			m_LocalUserId = other.LocalUserId.InnerHandle;
			m_MaxDataSizeBytes = other.MaxDataSizeBytes;
		}

		public void Dispose()
		{
			Helper.Dispose(ref m_LocalUserId);
			Helper.Dispose(ref m_RequestedChannel);
		}
	}
}
