Public Class Letter

    Public charCode As Integer
    Public numberOfContours As Integer
    Public minX As Integer
    Public minY As Integer
    Public maxX As Integer
    Public maxY As Integer
    Public type As String
    Public components As List(Of Component)
    Public contourEnds As List(Of Integer)
    Public flags As Integer
    Public aw As Integer
    Public lsb As Integer
    Public rsb As Integer

    Public lines As PointF()()

End Class

Public Class Component

    Public glyphIndex As Integer
    Public matrix As Matrix = New Matrix()
    Public destPointIndex As Integer
    Public srcPointIndex As Integer

End Class

Public Class Matrix

    Public a As Integer
    Public b As Integer
    Public c As Integer
    Public d As Integer
    Public e As Integer
    Public f As Integer

End Class
Public Class Vertex

    Public X As Single = 0
    Public Y As Single = 0
    Public type As String = ""

    Public Function clone()

        Dim vert = New Vertex()

        vert.X = X
        vert.Y = Y
        vert.type = type

        Return vert

    End Function

End Class