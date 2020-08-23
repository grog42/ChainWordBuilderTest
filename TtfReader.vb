Imports System.Drawing.Imaging
Imports System.IO
Imports System.Runtime.Remoting.Messaging

Public Class TtfReader

    Private reader As BinaryReader

    Private tables As Dictionary(Of String, UInt32)

    Private cmapOffset As Integer

    Private indexToLocFormat As Int16

    Public Sub New(buffer As Byte())

        reader = New BinaryReader(buffer)
        tables = readTableOffsets()
        readHeadTable()
        cmapOffset = getCmapOffset()

    End Sub

    Private Function readTableOffsets()

        Dim offsets As New Dictionary(Of String, UInt32)
        Dim scalarType = reader.getUint32()
        Dim tablesNum = reader.getUint16()
        Dim searchRange = reader.getUint16()
        Dim entrySelector = reader.getUint16()
        Dim rangeShift = reader.getUint16()

        For i As Integer = 1 To tablesNum

            Dim tag = reader.getString(4)
            Dim checksum = reader.getUint32()
            Dim offset = reader.getUint32()
            Dim length = reader.getUint32()

            offsets.Add(tag, offset)

            REM If tag = "head" AndAlso Not checksum = &HB1B0AFBA - calcTableChecksum(offset, length) Then

            REM Throw New Exception("Checksum incorrect")

            REM End If

        Next

        Return offsets

    End Function

    Private Function calcTableChecksum(offset, length) As UInt32

        Dim old = reader.setPtr(offset)
        Dim sum As UInt32 = 0
        Dim nlongs = ((length + 3) / 4) - 1

        Do While nlongs > 0
            sum += reader.getUint32()
            nlongs -= 1
        Loop

        reader.setPtr(old)
        Return sum

    End Function

    Private Sub readHeadTable()

        reader.setPtr(tables("head"))

        Dim version = reader.getFixed()
        Dim fontRevision = reader.getFixed()
        Dim checksumAdjustment = reader.getUint32()
        Dim magicNumber = reader.getUint32()
        Dim flags = reader.getUint16()
        Dim unitsPerEm = reader.getUint16()
        Dim created = reader.getDate()
        Dim modified = reader.getDate()
        Dim xMin = reader.getFword()
        Dim yMin = reader.getFword()
        Dim xMax = reader.getFword()
        Dim yMax = reader.getFword()
        Dim macStyle = reader.getUint16()
        Dim lowestRecPPEM = reader.getUint16()
        Dim fontDirectionHint = reader.getInt16()

        indexToLocFormat = reader.getInt16()

        Dim glyphDataFormat = reader.getInt16()

    End Sub

    Private Function getCmapOffset()

        reader.setPtr(tables("cmap"))

        Dim version = reader.getUint16()
        Dim tableNum = reader.getUint16()
        Dim count = 0
        Dim offset = 0
        Dim platformId As Integer
        Dim encodingId As Integer

        Do

            platformId = reader.getUint16()
            encodingId = reader.getUint16()
            offset = reader.getUint32()
            count += 1

        Loop While count < tableNum And (Not platformId = 3 And Not encodingId = 1)

        Return tables("cmap") + offset

    End Function

    Public Function getGlyph(charCode As Integer) As Letter

        Dim glyph As New Letter()
        glyph.charCode = charCode

        Dim index = getGlyphCmap(charCode)

        reader.setPtr(getGlyphOffset(index) + tables("glyf"))

        glyph.numberOfContours = reader.getInt16()
        glyph.minX = reader.getFword()
        glyph.minY = reader.getFword()
        glyph.maxX = reader.getFword()
        glyph.maxY = reader.getFword()

        If glyph.numberOfContours = -1 Then

            glyph = readCompoundGlyph(glyph)

        Else

            glyph = readSimpleGlyph(glyph)

        End If

        glyph = readHorMetric(glyph)
        Return glyph

    End Function

    Private Function readCompoundGlyph(ByRef glyph As Letter)

        Dim ARG_1_AND_2_ARE_WORDS = 1
        Dim ARGS_ARE_XY_VALUES = 2
        Dim ROUND_XY_TO_GRID = 4
        Dim WE_HAVE_A_SCALE = 8
        Dim RESERVED = 16
        Dim MORE_COMPONENTS = 32
        Dim WE_HAVE_AN_X_AND_Y_SCALE = 64
        Dim WE_HAVE_A_TWO_BY_TWO = 128
        Dim WE_HAVE_INSTRCTIONS = 256

        glyph.type = "compound"
        glyph.components = New List(Of Component)

        Dim flags = MORE_COMPONENTS

        Do While flags & MORE_COMPONENTS

            Dim arg1, arg2

            flags = reader.getUint16()

            Dim component = New Component
            component.glyphIndex = reader.getUint16()
            component.matrix.a = 1
            component.matrix.b = 0
            component.matrix.c = 0
            component.matrix.d = 1
            component.matrix.e = 0
            component.matrix.f = 0

            If flags & ARG_1_AND_2_ARE_WORDS Then

                arg1 = reader.getInt16()
                arg2 = reader.getInt16()

            Else

                arg1 = reader.getUint8()
                arg2 = reader.getUint8()

            End If

            If flags & ARGS_ARE_XY_VALUES Then

                component.matrix.e = arg1
                component.matrix.f = arg2

            Else

                component.destPointIndex = arg1
                component.srcPointIndex = arg2

            End If

            If flags & WE_HAVE_A_SCALE Then

                component.matrix.a = reader.get2Dot14()
                component.matrix.d = component.matrix.a

            ElseIf flags & WE_HAVE_AN_X_AND_Y_SCALE Then

                component.matrix.a = reader.get2Dot14()
                component.matrix.d = reader.get2Dot14()

            ElseIf flags & WE_HAVE_A_TWO_BY_TWO Then

                component.matrix.a = reader.get2Dot14
                component.matrix.b = reader.get2Dot14
                component.matrix.c = reader.get2Dot14
                component.matrix.d = reader.get2Dot14

            End If

            glyph.components.Add(component)

        Loop

        If flags & WE_HAVE_INSTRCTIONS Then

            reader.movPtr(reader.getUint16())

        End If

        Return glyph

    End Function

    Private Function readSimpleGlyph(ByRef glyph As Letter)

        Dim ON_CURVE As Byte = 1
        Dim X_IS_BYTE As Byte = 2
        Dim Y_IS_BYTE As Byte = 4
        Dim REPEAT As Byte = 8
        Dim X_DELTA As Byte = 16
        Dim Y_Delta As Byte = 32

        glyph.type = "simple"
        glyph.contourEnds = New List(Of Integer)
        Dim maxPointIndex = 0

        For i As Integer = 0 To glyph.numberOfContours - 1

            Dim index = reader.getUint16()

            glyph.contourEnds.Add(index)
            If maxPointIndex < index Then

                maxPointIndex = index

            End If
        Next

        Dim instructionLength = reader.getUint16()

        reader.movPtr(instructionLength)

        If glyph.numberOfContours = 0 Then

            Return glyph

        End If

        Dim points = New List(Of Vertex)
        Dim flags = New List(Of Integer)

        For i = 0 To maxPointIndex

            Dim flag As Byte = reader.getUint8()
            Dim point = New Vertex()
            flags.Add(flag)

            If flag And ON_CURVE Then

                point.type = "oncurve"

            Else

                point.type = "offcurve"

            End If

            points.Add(point)

            If flag And REPEAT Then

                Dim repeatCount As Integer = reader.getUint8()
                i += repeatCount

                For rep = 1 To repeatCount

                    flags.Add(flag)
                    points.Add(point.clone())

                Next

            End If

        Next

        Dim val As Integer = 0

        For i = 0 To points.Count - 1

            Dim flag = flags(i)

            If flag And X_IS_BYTE Then

                If flag And X_DELTA Then

                    val += reader.getUint8()

                Else

                    val -= reader.getUint8()

                End If

            ElseIf ((Not flag) And X_DELTA) Then

                val += reader.getInt16()

            End If

            points(i).X = val

        Next

        val = 0

        For i = 0 To points.Count - 1

            Dim flag = flags(i)

            If flag And Y_IS_BYTE Then

                If flag And Y_Delta Then

                    val += reader.getUint8()

                Else

                    val -= reader.getUint8()

                End If

            ElseIf ((Not flag) And Y_Delta) Then

                val += reader.getInt16()

            End If

            points(i).Y = val
        Next

        glyph.flags = reader.getUint16()

        glyph = BuildVertices(glyph, points.ToArray())

        Return glyph

    End Function

    Public Function BuildVertices(glyph As Letter, points As Vertex())

        glyph.lines = New PointF(glyph.contourEnds.Count - 1)() {}

        Dim minX = glyph.minX
        Dim minY = glyph.minY
        Dim line = New List(Of PointF)
        Dim first = True
        Dim startPoint = 0
        Dim c = 0

        For p = 0 To points.Length - 1

            If first Then

                line = New List(Of PointF)
                line.Add(normalise(points(p), minX, minY))
                startPoint = p
                first = False

            Else

                If points(p).type Is "oncurve" Then

                    line.Add(normalise(points(p), minX, minY))

                Else

                    Dim cpoints = New List(Of PointF)
                    Dim plusOne As Vertex

                    p -= 1
                    cpoints.Add(normalise(points(p), minX, minY))

                    Do
                        p += 1

                        cpoints.Add(normalise(points(p), minX, minY))

                        If p = glyph.contourEnds(c) Then

                            plusOne = points(startPoint)

                        Else

                            plusOne = points(p + 1)

                        End If

                        If plusOne.type Is "offcurve" Then

                            cpoints.Add(New PointF(points(p).X + ((plusOne.X - points(p).X) * 0.5) - minX, points(p).Y + ((plusOne.Y - points(p).Y) * 0.5) - minY))

                        End If

                    Loop Until plusOne.type Is "oncurve" Or p = glyph.contourEnds(c)

                    cpoints.Add(normalise(plusOne, minX, minY))

                    For i = 0 To cpoints.Count - 3 Step 2

                        line.AddRange(BuildCurve(cpoints(i), cpoints(i + 1), cpoints(i + 2)))

                    Next

                End If
            End If

            If p = glyph.contourEnds(c) Then

                line.Add(normalise(points(startPoint), minX, minY))
                glyph.lines(c) = line.ToArray()
                c += 1
                first = True

            End If

        Next

        Return glyph

    End Function

    Private Function normalise(p As Vertex, minX As Integer, minY As Integer)

        Return New PointF(p.X - minX, p.Y - minY)

    End Function

    Private Function BuildCurve(p1 As PointF, p2 As PointF, p3 As PointF) As List(Of PointF)

        Dim curve = New List(Of PointF)

        For t = 0 To 1 Step 0.02

            Dim x = (1 - t) ^ 2 * p1.X + 2 * (1 - t) * t * p2.X + t ^ 2 * p3.X
            Dim y = (1 - t) ^ 2 * p1.Y + 2 * (1 - t) * t * p2.Y + t ^ 2 * p3.Y
            curve.Add(New PointF(x, y))

        Next

        Return curve

    End Function

    Private Function readHorMetric(ByRef glyph As Letter)

        Dim index = getGlyphCmap(glyph.charCode)
        Dim offset = tables("hmtx")

        reader.setPtr(tables("hhea"))
        reader.movPtr(10)

        Dim advanceWidthMax = reader.getUint16()

        reader.movPtr(22)
        Dim HMetricCount = reader.getUint16()

        If index > HMetricCount Then

            offset += HMetricCount * 4 + (index - HMetricCount) * 2
            glyph.aw = advanceWidthMax

        Else

            offset += index * 4

        End If

        reader.setPtr(offset)

        glyph.aw = reader.getUint16()
        glyph.lsb = reader.getInt16()
        glyph.rsb = glyph.aw - (glyph.lsb + glyph.maxX - glyph.minX)

        Return glyph

    End Function

    Private Function getGlyphOffset(cmapIndex As Integer)

        Dim offset, old

        If indexToLocFormat = 1 Then
            old = reader.setPtr(tables("loca") + cmapIndex * 4)
            offset = reader.getUint32()

        Else

            old = reader.setPtr(tables("loca") + cmapIndex * 2)
            offset = reader.getUint16() * 2

        End If

        reader.setPtr(old)

        Return offset

    End Function

    Private Function getGlyphCmap(charCode As Integer) As Integer

        reader.setPtr(cmapOffset)
        reader.movPtr(6)

        Dim segCountX2 = reader.getUint16()

        reader.movPtr(6)

        Dim endCodeLoc = reader.getPtr()

        Dim endCode = 0
        Dim segmentIndex = -1

        Do While endCode < charCode

            segmentIndex += 1
            endCode = reader.getUint16()

        Loop

        reader.setPtr(endCodeLoc + segCountX2)
        reader.movPtr(2)

        Dim startLoc = reader.getPtr()
        reader.movPtr(segmentIndex * 2)

        Dim startCode = reader.getUint16()
        reader.setPtr(startLoc + segCountX2)

        Dim idDeltaLoc = reader.getPtr()
        reader.setPtr(idDeltaLoc + (segmentIndex * 2))

        Dim idDelta = reader.getInt16()
        reader.setPtr(idDeltaLoc + segCountX2)
        reader.movPtr(segmentIndex * 2)

        Dim idRangeOffset = reader.getUint16()

        If idRangeOffset = 0 Then

            Return charCode + idDelta

        End If

        reader.movPtr(idRangeOffset + 2 * (charCode - startCode) - 2)

        Return reader.getUint16() + idDelta

    End Function

End Class
