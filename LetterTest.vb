Imports System.IO

Public Class LetterTest

    Private fontPath = "D:\Documents\WorkSpace\InfLetterTest\bin\Fonts"
    Private letters As Dictionary(Of Integer, Letter) = New Dictionary(Of Integer, Letter)
    Private fontFile As TtfReader

    Private Sub LetterTest_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim fileNames As String() = IO.Directory.GetFiles(fontPath)

        For i = 0 To fileNames.Length - 1

            If fileNames(i).Contains("ttf") Or fileNames(i).Contains("TTF") Then

                FontSelector.Items.Add(fileNames(i).Replace(fontPath + "\", ""))

            End If
        Next

    End Sub

    Private Sub TextInput_TextChanged(sender As Object, e As EventArgs) Handles TextInput.TextChanged

        Refresh()

    End Sub

    Private Sub PictureBox1_Paint(sender As Object, e As PaintEventArgs) Handles MainDisplay.Paint

        If FontSelector.SelectedItem IsNot Nothing And TextInput.Text IsNot "" Then

            For Each c As Char In TextInput.Text

                If Not letters.ContainsKey(Asc(c)) Then

                    letters.Add(Asc(c), fontFile.getGlyph(Asc(c)))

                End If

            Next

            Dim textWidth = 0
            Dim overlap = SpacingSelector.Value
            Dim text = TextInput.Text

            For i = 0 To text.Length - 1

                textWidth += letters(Asc(text(i))).aw - overlap

            Next

            Dim scale = e.ClipRectangle.Width / textWidth
            Dim g = e.Graphics
            Dim totalWidth = 0

            For i = 0 To text.Length - 1

                Dim c = Asc(text(i))
                Dim lines = letters(c).lines

                For l = 0 To lines.Length - 1

                    Dim verts = New List(Of PointF)

                    For p = 0 To lines(l).Length - 1

                        verts.Add(New PointF((lines(l)(p).X + totalWidth + letters(c).lsb - overlap) * scale, lines(l)(p).Y * scale))

                    Next

                    g.DrawLines(New Pen(Color.Black), verts.ToArray())

                Next

                totalWidth += letters(c).aw - overlap

            Next

        End If

    End Sub

    Private Sub SpacingSelector_ValueChanged(sender As Object, e As EventArgs) Handles SpacingSelector.ValueChanged

        Refresh()

    End Sub

    Private Sub FontSelector_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FontSelector.SelectedIndexChanged

        letters = New Dictionary(Of Integer, Letter)
        fontFile = New TtfReader(My.Computer.FileSystem.ReadAllBytes(fontPath + "\" + FontSelector.SelectedItem))
        Refresh()

    End Sub
End Class
