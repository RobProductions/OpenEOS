// Copyright Epic Games, Inc. All Rights Reserved.

using System;

namespace Epic.OnlineServices.P2P
{
	public sealed partial class P2PInterface : Handle
	{
		/// <summary>
		/// Receive the next packet for the local user, and information associated with this packet, if it exists.
		/// <seealso cref="GetNextReceivedPacketSize" />
		/// </summary>
		/// <param name="options">Information about who is requesting the size of their next packet, and how much data can be stored safely</param>
		/// <param name="outPeerId">The Remote User who sent data. Only set if there was a packet to receive.</param>
		/// <param name="outSocketId">The Socket ID of the data that was sent. Only set if there was a packet to receive.</param>
		/// <param name="outChannel">The channel the data was sent on. Only set if there was a packet to receive.</param>
		/// <param name="outData">Buffer to store the data being received. Must be at least <see cref="GetNextReceivedPacketSize" /> in length or data will be truncated</param>
		/// <param name="outBytesWritten">The amount of bytes written to OutData. Only set if there was a packet to receive.</param>
		/// <returns>
		/// <see cref="Result.Success" /> - If the packet was received successfully
		/// <see cref="Result.InvalidParameters" /> - If input was invalid
		/// <see cref="Result.NotFound" /> - If there are no packets available for the requesting user
		/// </returns>
		public Result ReceivePacket(ref ReceivePacketOptions options, ref ProductUserId outPeerId, ref SocketId outSocketId, out byte outChannel, System.ArraySegment<byte> outData, out uint outBytesWritten)
		{ 
			bool wasCacheValid = outSocketId.PrepareForUpdate();
			IntPtr outSocketIdAddr = Helper.AddPinnedBuffer(outSocketId.m_AllBytes);
			IntPtr outDataAddress = Helper.AddPinnedBuffer(outData);
			var optionsInternal = new ReceivePacketOptionsInternal(ref options);
			try
			{
				var outPeerIdAddress = IntPtr.Zero;
				outChannel = default;
				outBytesWritten = 0;
				var funcResult = Bindings.EOS_P2P_ReceivePacket(InnerHandle, ref optionsInternal, out outPeerIdAddress, outSocketIdAddr, out outChannel, outDataAddress, out outBytesWritten);

				if (outPeerId == null)
				{
					// Note: Will allocate a new ProductUserId(), to avoid continual allocation of a peer id object, pass a non null reference.
					Helper.Get(outPeerIdAddress, out outPeerId);
				}
				else if (outPeerId.InnerHandle != outPeerIdAddress)
				{
					// Optimization Note: clients can pass the same ProductUserId object to avoid continually allocating a new object but will need to pay attention to InnerHandler changes
					outPeerId.InnerHandle = outPeerIdAddress;
				}

				// Optimization Note: this will check if socket ID bytes were unchanged to allow using previous cached string and avoid a new allocation.
				outSocketId.CheckIfChanged(wasCacheValid);

				return funcResult;
			}
			finally
			{
				Helper.Dispose(ref outSocketIdAddr);
				Helper.Dispose(ref outDataAddress);
				optionsInternal.Dispose();
			}
		}
	}
}
