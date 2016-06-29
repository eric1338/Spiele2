using System;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK;

namespace Framework
{
	[Serializable]
	public class VAOException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="VAOException"/> class.
		/// </summary>
		/// <param name="msg">The error msg.</param>
		public VAOException(string msg) : base(msg) { }
	}

	public class VAO : IDisposable
	{
		public VAO()
		{
			idVAO = GL.GenVertexArray();
		}

		public void Dispose()
		{
			foreach (uint id in boundBuffers.Values)
			{
				GL.DeleteBuffer(id);
			}
			boundBuffers.Clear();
			GL.DeleteVertexArray(idVAO);
			idVAO = 0;
		}

		public void SetID<Index>(Index[] data, PrimitiveType primitiveType) where Index : struct
		{
			Activate();
			uint bufferID = RequestBuffer(idBufferBinding);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, bufferID);
			var type = typeof(Index);
			int elementBytes = Marshal.SizeOf(type);
			int bufferByteSize = data.Length * elementBytes;
			// set buffer data
			GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)bufferByteSize, data, BufferUsageHint.StaticDraw);
			//cleanup state
			Deactive();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			//save data for draw call
			DrawElementsType drawElementsType = GetDrawElementsType(type);
			idData = new IDData(primitiveType, data.Length, drawElementsType);
		}

		public void SetAttribute<DataElement>(int bindingID, DataElement[] data, VertexAttribPointerType type, int elementSize, bool perInstance = false) where DataElement : struct
		{
			if (-1 == bindingID) return; //if attribute not used in shader or wrong name
			Activate();
			uint bufferID = RequestBuffer(bindingID);
			GL.BindBuffer(BufferTarget.ArrayBuffer, bufferID);
			int elementBytes = Marshal.SizeOf(typeof(DataElement));
			int bufferByteSize = data.Length * elementBytes;
			// set buffer data
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)bufferByteSize, data, BufferUsageHint.StaticDraw);
			//set data format
			GL.VertexAttribPointer(bindingID, elementSize, type, false, elementBytes, 0);
			GL.EnableVertexAttribArray(bindingID);
			if (perInstance)
			{
				GL.VertexAttribDivisor(bindingID, 1);
			}
			//cleanup state
			Deactive();
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.DisableVertexAttribArray(bindingID);
		}

		/// <summary>
		/// sets or updates a vertex attribute of type Matrix4
		/// </summary>
		/// <param name="bindingID">shader binding location</param>
		/// <param name="data">ATTENTION: here the matrices are assumed to be rowmajor. why i don't know</param>
		/// <param name="perInstance"></param>
		public void SetMatrixAttribute(int bindingID, Matrix4[] data, bool perInstance = false)
		{
			if (-1 == bindingID) return; //if matrix not used in shader or wrong name
			Activate();
			uint bufferID = RequestBuffer(bindingID);
			GL.BindBuffer(BufferTarget.ArrayBuffer, bufferID);
			int elementBytes = Marshal.SizeOf(typeof(Matrix4));
			int columnBytes = Marshal.SizeOf(typeof(Vector4));
			int bufferByteSize = data.Length * elementBytes;
			// set buffer data
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)bufferByteSize, data, BufferUsageHint.StaticDraw);
			//set data format
			for (int i = 0; i < 4; i++)
			{
				GL.VertexAttribPointer(bindingID + i, 4, VertexAttribPointerType.Float, false, elementBytes, columnBytes * i);
				GL.EnableVertexAttribArray(bindingID + i);
				if (perInstance)
				{
					GL.VertexAttribDivisor(bindingID + i, 1);
				}
			}
			//cleanup state
			Deactive();
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			for (int i = 0; i < 4; i++)
			{
				GL.DisableVertexAttribArray(bindingID + i);
			}
		}

		public void Activate()
		{
			GL.BindVertexArray(idVAO);
		}

		public void Deactive()
		{
			GL.BindVertexArray(0);
		}

		public void DrawArrays(PrimitiveType type, int count, int start = 0)
		{
			Activate();
			GL.DrawArrays(type, start, count);
			Deactive();
		}

		public void Draw(int instanceCount = 1)
		{
			if (0 == idData.length) throw new VAOException("Empty id data set! Draw yourself using active/deactivate!");
			Activate();
			GL.DrawElementsInstanced(idData.primitiveType, idData.length, idData.drawElementsType, (IntPtr)0, instanceCount);
			Deactive();
		}

		private struct IDData
		{
			public DrawElementsType drawElementsType;
			public int length;
			public PrimitiveType primitiveType;

			public IDData(PrimitiveType primitiveType, int length, DrawElementsType drawElementsType)
			{
				this.primitiveType = primitiveType;
				this.length = length;
				this.drawElementsType = drawElementsType;
			}
		}

		private IDData idData;
		private int idVAO;
		private const int idBufferBinding = int.MaxValue;
		private Dictionary<int, uint> boundBuffers = new Dictionary<int, uint>();

		private static DrawElementsType GetDrawElementsType(Type type)
		{
			if (type == typeof(ushort)) return DrawElementsType.UnsignedShort;
			if (type == typeof(uint)) return DrawElementsType.UnsignedInt;
			throw new Exception("Invalid index type");
		}

		private uint RequestBuffer(int bindingID)
		{
			uint bufferID;
			if (!boundBuffers.TryGetValue(bindingID, out bufferID))
			{
				GL.GenBuffers(1, out bufferID);
				boundBuffers[bindingID] = bufferID;
			}
			return bufferID;
		}
	}
}
