Imports System.Collections
Public Class BinaryReader

    Private buffer As Byte()
    Private ptr As Integer

    Public Sub New(_Buffer As Byte())

        buffer = _Buffer
        ptr = 0

    End Sub

    Public Function setPtr(val As Integer)

        Dim oldPtr = ptr
        ptr = val
        Return oldPtr

    End Function

    Public Function getPtr()

        Return ptr

    End Function

    Public Sub movPtr(val As Integer)

        ptr += val

    End Sub

    Public Function getUint8() As Byte

        ptr += 1
        Return buffer(ptr - 1)

    End Function

    Public Function getUint16() As UInt16

        Dim n1 As UShort = getUint8()
        Dim n2 As UShort = getUint8()

        Return (n1 << 8 Or n2)

    End Function

    Public Function getInt16() As Int16

        Dim n1 As Short = getUint8()
        Dim n2 As Short = getUint8()

        Return (n1 << 8 Or n2)

    End Function

    Public Function getUint32() As UInt32

        Dim n1 As UInteger = getUint8()
        Dim n2 As UInteger = getUint8()
        Dim n3 As UInteger = getUint8()
        Dim n4 As UInteger = getUint8()

        Return ((n1 << 24) Or (n2 << 16) Or (n3 << 8) Or n4)

    End Function

    Public Function getInt32() As Int32

        Dim n1 As UInteger = getUint8()
        Dim n2 As UInteger = getUint8()
        Dim n3 As UInteger = getUint8()
        Dim n4 As UInteger = getUint8()

        Return ((n1 << 24) Or (n2 << 16) Or (n3 << 8) Or n4)

    End Function

    Public Function getString(length As Integer) As String

        Dim chars = New Char(length - 1) {}

        For i = 0 To length - 1

            chars(i) = Convert.ToChar(buffer(ptr))
            ptr += 1

        Next

        Return New String(chars)

    End Function

    Public Function getFword()

        Return getInt16()

    End Function

    Public Function get2Dot14()

        Return getInt16() / (1 << 14)

    End Function

    Public Function getFixed()

        Return getInt32() / (1 << 16)

    End Function

    Public Function getDate()

        Dim d = New DateTime(1904, 1, 1)

        Return d.AddSeconds((getUint32() << 32) + getUint32())

    End Function

End Class
