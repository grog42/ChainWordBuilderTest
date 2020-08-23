Imports System.IO

Module ChainNameBuilder

    Public Sub DrawChain(e As PaintEventArgs, fontPath As String, text As String, overlap As Integer)

        Dim buffer As Byte() = My.Computer.FileSystem.ReadAllBytes(fontPath)
        Dim letters As Letter() = New Letter(text.Length - 1) {}
        Dim ttf = New TtfReader(buffer)
        Dim textWidth = 0

        For i = 0 To text.Length - 1

            letters(i) = ttf.getGlyph(Asc(text(i)))
            textWidth += letters(i).aw - overlap

        Next

        Dim scale = e.ClipRectangle.Width / textWidth
        Dim g = e.Graphics
        Dim totalWidth = 0

        For i = 0 To letters.Length - 1

            Dim lines = letters(i).lines

            For l = 0 To lines.Length - 1

                Dim verts = New List(Of PointF)

                For p = 0 To lines(l).Length - 1

                    verts.Add(New PointF((lines(l)(p).X + totalWidth + letters(i).lsb - overlap) * scale, (lines(l)(p).Y + totalWidth) * scale))

                Next

                g.DrawLines(New Pen(Color.Black), verts.ToArray())

            Next

            totalWidth += letters(i).aw - overlap

        Next

    End Sub

End Module
