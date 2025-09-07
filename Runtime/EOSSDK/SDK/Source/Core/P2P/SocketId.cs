// Copyright Epic Games, Inc. All Rights Reserved.

using System;
using System.Text;

namespace Epic.OnlineServices.P2P
{
	/// <summary>
	/// P2P Socket ID
	/// 
	/// The Socket ID contains an application-defined name for the connection between a local person and another peer.
	/// 
	/// When a remote user receives a connection request from you, they will receive this information. It can be important
	/// to only accept connections with a known socket-name and/or from a known user, to prevent leaking of private
	/// information, such as a user's IP address. Using the socket name as a secret key can help prevent such leaks. Shared
	/// private data, like a private match's Session ID are good candidates for a socket name.
	/// </summary>
	public struct SocketId
	{
		public static readonly SocketId Empty = new SocketId();
		private const int MaxSocketNameLength = 32;
		private const int ApiVersionLength = sizeof(int);
		private const int NullTerminatorSpace = 1;
		private const int TotalSizeInBytes = MaxSocketNameLength + ApiVersionLength + NullTerminatorSpace;

		private bool m_CacheValid;
		private string m_CachedSocketName;

		internal byte[] m_AllBytes;
		internal byte[] m_SwapBuffer;

		public string SocketName
		{
			get
			{
				if (m_CacheValid)
				{
					return m_CachedSocketName;
				}

				if (m_AllBytes == null)
				{
					return null;
				}

				RebuildStringFromBuffer();

				return m_CachedSocketName;
			}
			set
			{
				m_CachedSocketName = value;
				if(value == null)
				{
					m_CacheValid = true;
					return;
				}

				EnsureStorage();

				int stringEndIndex = Math.Min(MaxSocketNameLength, value.Length);
				ASCIIEncoding.ASCII.GetBytes(value, 0, stringEndIndex, m_AllBytes, ApiVersionLength);
				// Add ascii null to end of string
				m_AllBytes[stringEndIndex + ApiVersionLength] = 0;
				m_CacheValid = true;
			}
		}

		internal bool PrepareForUpdate()
		{
			bool wasCacheValid = m_CacheValid;
			m_CacheValid = false;
			EnsureStorage();
			CopyIdToSwapBuffer();
			return wasCacheValid;
		}

		internal void CheckIfChanged(bool wasCacheValid)
		{
			if (!wasCacheValid || m_SwapBuffer == null || m_AllBytes == null)
			{
				return;
			}

			bool identical = true;
			for (int i = 0; i < m_SwapBuffer.Length; i++)
			{
				if (m_AllBytes[ApiVersionLength + i] != m_SwapBuffer[i])
				{
					identical = false;
					break;
				}
			}

			if (identical)
			{
				// No need to recompute the value of cached string
				m_CacheValid = true;
			}
		}

		private void RebuildStringFromBuffer()
		{
			EnsureStorage();

			int stringEndIndex;
			for (stringEndIndex = ApiVersionLength; stringEndIndex < m_AllBytes.Length && m_AllBytes[stringEndIndex] != '\0'; stringEndIndex++)
			{
			}

			m_CachedSocketName = ASCIIEncoding.ASCII.GetString(m_AllBytes, ApiVersionLength, stringEndIndex - ApiVersionLength);
			m_CacheValid = true;
		}

		private void EnsureStorage()
		{
			if (m_AllBytes == null || m_AllBytes.Length < TotalSizeInBytes)
			{
				m_AllBytes = new byte[TotalSizeInBytes];
				m_SwapBuffer = new byte[TotalSizeInBytes - ApiVersionLength];
				Array.Copy(BitConverter.GetBytes(P2PInterface.SOCKETID_API_LATEST), 0, m_AllBytes, 0, sizeof(int));
			}
		}

		private void CopyIdToSwapBuffer()
		{
			Array.Copy(m_AllBytes, ApiVersionLength, m_SwapBuffer, 0, m_SwapBuffer.Length);
		}
	}
}
