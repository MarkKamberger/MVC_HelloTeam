Imports System.Configuration.ConfigurationManager
Imports System.Data
Imports Telerik.Web.UI
Imports SFAFGlobalObjects
Imports SALIBusinessLogic
Imports MemberCenter20NS

Partial Public Class Cycles
    Inherits SFAFMemberCenter20ContentPage

    'Private MyError As SFAFMemberCenterDAL.Errors
    Private iSchoolTrackId As Integer
    Private bIsMissingTracks As Boolean
    Private bIsMissingGradingPeriods As Boolean
    Private bIsMissingClassrooms As Boolean
    Private businessLogic As SALIMainBusinessLogic
    Public iClassroomAssignmentId As Integer

#Region "PAGE_LOAD"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        businessLogic = CType(Me.Master, ISFAFMemberCenter20Master).BusinessLogicObject

        'CLEAR OUT THE GRID ON EVERY PAGE LOAD
        RadGrid1.DataSource = Nothing

        If Not IsPostBack Then
            'CType(Page.Master, MCHelp).HelpCenterVisible = False
            'CType(Page.Master, MCHelp).PageTitle = "<a href=Home.aspx><u>Classroom Center</u></a> > Lesson Cycles"
            InitializeForm()
        End If


        RadWindowTitles.NavigateUrl = "Titles.aspx?ClassroomAssignmentId=" + GetSelectedClassroomAssignmentId().ToString

        SetPrintLink()
        SetUpNewLessonLink()

        'Session("LessonClassroomIndex") = ddlClassrooms.SelectedIndex
        'Dim values() As String = ddlClassrooms.SelectedValue.ToString.Split(",")

        'If (values.Length > 1) Then
        '    ClassroomAssignmentId.Value = values(1)
        'End If

    End Sub

    Private Sub InitializeForm()
        SetCurrentGradingPeriod()
        LoadDDLs()
        SetFormVisibility()
    End Sub

    Private Sub SetFormVisibility()
        'SETS UP FORM CONTROL VISIBILITY DEPENDING ON PREREQUISITE CHECKS
        If bIsMissingTracks Then
            PanelLessonsForm.Visible = False
            PanelPrerequisites.Visible = True
            divTracksWarning.Visible = True
        Else
            divTracksWarning.Visible = False
        End If

        If bIsMissingGradingPeriods Then
            PanelLessonsForm.Visible = False
            PanelPrerequisites.Visible = True
            divGradingPeriodsWarning.Visible = True
        Else
            divGradingPeriodsWarning.Visible = False
        End If
    End Sub

    Private Sub SetCurrentGradingPeriod()
        'CHECK TO SEE IF THE USER HAS CHANGED THE CURRENT GRADING PERIOD
        'Dim oCustomers As New SFAFMemberCenterDAL.Customers
        Dim iGradingPeriodId As Integer = 0
        If Session("LessonGradingPeriod") = 0 Then
            'CHECK TO SEE IF THE CURRENT SCHOOL HAS A CURRENT GRADING PERIOD DEFINED AND SET IT AS THE DEFAULT GRADING PERIOD
            'iGradingPeriodId = oCustomers.GetSchoolCurrentGradingPeriod(ConnectionStrings("MembersDBConnection").ToString, CInt(Session("CurrentCustomerId")), CInt(Session("StaffId")), Session("FullName"), ConnectionStrings("BusinessMgmt").ToString)
            iGradingPeriodId = businessLogic.GetCurrentGradingPeriod(CInt(Session("CurrentCustomerId")))
            If iGradingPeriodId <> 0 Then
                Session("LessonGradingPeriod") = iGradingPeriodId
            End If
        End If
    End Sub
#End Region

#Region "DDLs & GRIDS"
    Private Sub LoadDDLs()
        Dim dtSchoolYears As New DataTable
        Dim dtTracks As New DataTable
        Dim dtGdPds As New DataTable
        Dim so As SALIObject

        Try
            'LOAD SCHOOL YEARS
            so = businessLogic.GetSchoolYears()

            LoadDDL(so.ObjectData, ddlSchoolYear, Session("LessonSchoolYearId"))

            'LOAD AND SHOW THE TRACKS FILTER IF THE SCHOOL HAS MORE THAN 0 TRACKS
            If CInt(Session("Tracks")) > 0 Then
                'dtTracks = o.GetDataWith2IntOptionalParams(ConnectionStrings("MembersDBConnection").ToString, "LookupSchoolTracks", "@SchoolId", CInt(Session("CurrentCustomerId")), "", 0, Session("StaffId"), Session("FullName"), ConnectionStrings("BusinessMgmt").ToString)
                If dtTracks.Rows.Count > 0 Then
                    Dim r As DataRow
                    For Each r In dtTracks.Rows
                        Dim li As New ListItem
                        li.Text = r("TrackName")
                        li.Value = r("SchoolTrackId")
                        ddlSchoolTrack.Items.Add(li)
                    Next

                    'TRY SELECTING LAST SELECTED VALUE FROM THE SESSION
                    Try
                        If Session("LessonTrack") <> "" Then
                            ddlSchoolTrack.SelectedValue = Session("LessonTrack")
                        Else
                            ddlSchoolTrack.SelectedIndex = 0
                        End If
                    Catch ex As Exception

                    End Try
                    iSchoolTrackId = ddlSchoolTrack.SelectedValue 'NEEDED TO LOAD THE CLASSROOMS FILTER
                Else
                    'THIS IS A TRACK SCHOOL BUT NO TRACKS HAVE BEEN SETUP YET FOR THE CURRENT SCHOOL YEAR
                    bIsMissingTracks = True
                End If
                'SET VISIBILITY OF THE TRACKS FILTER
                ddlSchoolTrack.Visible = True
                tdTrackLabel.Visible = True
                tdTrackValue.Visible = True
            Else
                'SET VISIBILITY OF THE TRACKS FILTER
                ddlSchoolTrack.Visible = False
                tdTrackLabel.Visible = False
                tdTrackValue.Visible = False
            End If

            'LOAD THE GRADING PERIODS FILTER
            so = businessLogic.GetGradingPeriods(CInt(Session("CurrentCustomerId")), 2)

            If (so.ObjectDataRows > 0) Then
                LoadDDL(so.ObjectData, ddlGradingPeriod, Session("LessonGradingPeriod"))
            Else
                'THE SCHOOL DOES NOT HAVE ANY GRADING PERIODS SETUP YET FOR THE CURRENT SCHOOL YEAR
                bIsMissingGradingPeriods = True
            End If

            'LoadClassroomsDDL()
            Page.DataBind()
        Catch ex As Exception
            businessLogic.LogError(ex)
            Response.Redirect(AppSettings("ErrorPage").ToString)
        Finally
            dtTracks = Nothing
            dtGdPds = Nothing
            'o = Nothing
        End Try
    End Sub

    Protected Sub ddlSchoolTrack_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlSchoolTrack.SelectedIndexChanged
        Session("LessonTrack") = ddlSchoolTrack.SelectedValue
        iSchoolTrackId = ddlSchoolTrack.SelectedValue
        Session("LessonGradingPeriod") = 0
        Session("LessonClassroomIndex") = ""
        LoadClassroomsDDL()
    End Sub

    Private Sub ddlSchoolYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSchoolYear.SelectedIndexChanged
        Session("LessonSchoolYearId") = ddlSchoolYear.SelectedValue
        Session("LessonClassroomIndex") = ""
        LoadClassroomsDDL()
    End Sub

    Protected Sub ddlGradingPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlGradingPeriod.SelectedIndexChanged
        Session("LessonGradingPeriod") = ddlGradingPeriod.SelectedValue
        Session("LessonClassroomIndex") = ""
        LoadClassroomsDDL()
    End Sub

    Protected Sub ddlClassrooms_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlClassrooms.SelectedIndexChanged
        Session("LessonClassroomIndex") = ddlClassrooms.SelectedIndex
        Dim values() As String = ddlClassrooms.SelectedValue.ToString.Split(",")

        If (values.Length > 1) Then
            ClassroomAssignmentId.Value = values(1)
            iClassroomAssignmentId = values(1)
        End If

        If (IsReadingRoots()) Then
            lnkBtnFTPAssessment.Visible = True
        Else
            lnkBtnFTPAssessment.Visible = False
        End If

        If (IsStrategyUse()) Then
            lnkBtnStragegyAssessment.Visible = True
        Else
            lnkBtnStragegyAssessment.Visible = False
        End If

        If (IsReadingWings4()) Then
            IsReadingWings4Field.Value = "true"
        Else
            IsReadingWings4Field.Value = "false"
        End If

        LoadGrid()
    End Sub

    Private Sub LoadGrid()
        'Dim oClassrooms As New SFAFMemberCenterDAL.Classrooms
        Dim dt As DataTable
        Try
            RadGrid1.MasterTableView.Columns.Clear()
            Dim c1 As New Telerik.Web.UI.GridBoundColumn
            c1.Groupable = False
            c1.HeaderText = "Start Date"
            c1.SortExpression = "LessonStartsOn"
            c1.HeaderButtonType = Telerik.Web.UI.GridHeaderButtonType.TextButton
            c1.DataField = "LessonStartsOn"
            c1.UniqueName = "LessonStartsOn"
            c1.DataFormatString = "{0:MM/dd/yy}"
            RadGrid1.MasterTableView.Columns.Add(c1)

            If IsReadingRoots() Then
                Dim c1a As New Telerik.Web.UI.GridBoundColumn
                c1a.Groupable = False
                c1a.HeaderText = "Level"
                c1a.SortExpression = "Level"
                c1a.HeaderButtonType = Telerik.Web.UI.GridHeaderButtonType.TextButton
                c1a.DataField = "Level"
                c1a.UniqueName = "Level"
                RadGrid1.MasterTableView.Columns.Add(c1a)
            End If

            If (True) Then 'businessLogic.CheckUserObjectPermission(Session("SFAFUserName"), SALISecurityObjectTypes.Lesson.ToString(), SFAFSecurityLevel.SELECT, SFAFSecurityScope.SELF)) Then
                Dim c2 As New Telerik.Web.UI.GridHyperLinkColumn
                c2.Groupable = False
                c2.HeaderText = "Lesson Cycles"
                c2.SortExpression = "Lesson"
                c2.HeaderButtonType = Telerik.Web.UI.GridHeaderButtonType.TextButton
                Dim sUrlFields() As String = Split("ClassScoresheetId", ",")
                c2.DataNavigateUrlFields = sUrlFields
                'If IsReadingRoots() Then
                'c2.DataNavigateUrlFormatString = "CycleEditRoots.aspx?LessonId={0}"
                'Else
                c2.DataNavigateUrlFormatString = "CycleEdit.aspx?LessonId={0}"
                'End If
                c2.DataTextField = "Lesson"
                c2.UniqueName = "Lesson"
                RadGrid1.MasterTableView.Columns.Add(c2)
            Else
                Dim c2 As New Telerik.Web.UI.GridBoundColumn
                c2.Groupable = False
                c2.HeaderText = "Lesson Cycles"
                c2.SortExpression = "Lesson"
                c2.HeaderButtonType = Telerik.Web.UI.GridHeaderButtonType.TextButton
                c2.DataField = "Lesson"
                c2.UniqueName = "Lesson"
                RadGrid1.MasterTableView.Columns.Add(c2)
            End If

            If IsReadingRoots() Then
                Dim c2a As New Telerik.Web.UI.GridBoundColumn
                c2a.Groupable = False
                c2a.HeaderText = "FTP Assessment"
                c2a.SortExpression = "FTP Assessment"
                c2a.HeaderButtonType = Telerik.Web.UI.GridHeaderButtonType.TextButton
                c2a.DataField = "FTP Assessment"
                c2a.UniqueName = "FTP Assessment"
                RadGrid1.MasterTableView.Columns.Add(c2a)
            End If

            Dim c3 As New Telerik.Web.UI.GridBoundColumn
            c3.Groupable = False
            c3.HeaderText = "Classroom"
            c3.SortExpression = "Class"
            c3.HeaderButtonType = Telerik.Web.UI.GridHeaderButtonType.TextButton
            c3.DataField = "Class"
            c3.UniqueName = "Class"
            RadGrid1.MasterTableView.Columns.Add(c3)

            Dim c4 As New Telerik.Web.UI.GridBoundColumn
            c4.Groupable = False
            c4.HeaderText = "Strategy Target"
            c4.SortExpression = "LessonStrategy"
            c4.HeaderButtonType = Telerik.Web.UI.GridHeaderButtonType.TextButton
            c4.DataField = "LessonStrategy"
            c4.UniqueName = "LessonStrategy"
            RadGrid1.MasterTableView.Columns.Add(c4)

            Dim c5 As New Telerik.Web.UI.GridBoundColumn
            c5.Groupable = False
            c5.HeaderText = "Sub-Strategy"
            c5.SortExpression = "LessonSubStrategy"
            c5.HeaderButtonType = Telerik.Web.UI.GridHeaderButtonType.TextButton
            c5.DataField = "LessonSubStrategy"
            c5.UniqueName = "LessonSubStrategy"
            RadGrid1.MasterTableView.Columns.Add(c5)

            'Dim c6 As New Telerik.Web.UI.GridTemplateColumn
            'c6.Groupable = False
            'c6.HeaderText = "Status"
            'c6.SortExpression = "LessonIsOpen"
            'c6.HeaderButtonType = GridHeaderButtonType.TextButton
            'c6.DataField = "LessonIsOpen"
            'c6.UniqueName = "LessonStatusImage"
            'c6.ItemTemplate = New GridTemplate("LessonIsOpen", "LessonStatusImage")
            'RadGrid1.MasterTableView.Columns.Add(c6)

            'LOAD DATA
            Dim iClassroomAssignmentId As Integer = GetSelectedClassroomAssignmentId()

            If CInt(Session("Tracks")) > 0 Then
                iSchoolTrackId = ddlSchoolTrack.SelectedValue
            Else
                iSchoolTrackId = 0
            End If

            dt = businessLogic.GetClassroomLongLessons(CInt(Session("CurrentCustomerId")), CInt(Session("LessonSchoolYearId")), iSchoolTrackId, ddlGradingPeriod.SelectedValue, iClassroomAssignmentId).ObjectData

            RadGrid1.DataSource = dt
            RadGrid1.DataBind()
            SetupGridBasedOnCurrentlySelectedProduct()

        Catch ex As Exception
            'MyError = New SFAFMemberCenterDAL.Errors
            'MyError.InsertError(ConnectionStrings("BusinessMgmt").ToString, Me.ToString + " LoadGrid", ex.ToString, ex.StackTrace.ToString, Session("StaffID"), Session("FullName"))
            'MyError = Nothing
            Response.Redirect(AppSettings("ErrorPage").ToString)
        Finally
            dt = Nothing
            'oClassrooms = Nothing
        End Try
    End Sub

    Private Function GetSelectedClassroomAssignmentId() As Integer
        Dim iClassroomAssignmentId As Integer = 0
        Try

            If ddlClassrooms.SelectedValue.ToString <> "" Then
                Dim s() As String = Split(ddlClassrooms.SelectedValue.ToString, ",")
                iClassroomAssignmentId = CInt(s(1))
            End If
        Catch ex As Exception

        End Try
        Return iClassroomAssignmentId
    End Function

    Private Sub SetupGridBasedOnCurrentlySelectedProduct()
        'ADDITIONAL WORK TO SETUP GRID BASED ON THE PRODUCT
        'GET PRODUCT ID FROM CURRENTLY SELECTED CLASSROOM ASSIGNMENT
        Try
            Dim iClassroomAssignmentId As Integer = 0
            If ddlClassrooms.SelectedValue.ToString <> "" Then
                Dim s() As String = Split(ddlClassrooms.SelectedValue.ToString, ",")
                Dim sProductId As String = s(0).ToString

                If IsReadingEdgeHighSchool() Then
                    RadGrid1.MasterTableView.Columns(3).Visible = False
                    RadGrid1.MasterTableView.Columns(4).Visible = False
                ElseIf IsReadingRoots() Then
                    RadGrid1.MasterTableView.Columns(5).Visible = False
                    RadGrid1.MasterTableView.Columns(6).Visible = False
                End If

            End If
        Catch ex As Exception
            'MyError = New SFAFMemberCenterDAL.Errors
            'MyError.InsertError(ConnectionStrings("BusinessMgmt").ToString, Me.ToString + " SetupGridBasedOnCurrentlySelectedProduct", ex.ToString, ex.StackTrace.ToString, Session("StaffID"), Session("FullName"))
            'MyError = Nothing
            Response.Redirect(AppSettings("ErrorPage").ToString)
        End Try
    End Sub

    Private Sub LoadClassroomsDDL()
        Dim dtClass As New DataTable
        'Dim oClassrooms As New SFAFMemberCenterDAL.Classrooms
        Dim teacherId As Integer = -1

        Try
            'LOAD THE CLASSROOMS FILTER (HAS A BLANK OPTION TO SHOW ALL)
            ddlClassrooms.Items.Clear()

            If CInt(Session("Tracks")) > 0 Then
                iSchoolTrackId = ddlSchoolTrack.SelectedValue
            Else
                iSchoolTrackId = 0
            End If

            If Session("CurrentPageSecurityLevel").ToString = "6" Then
                'FILTER RESULTS BY TEACHER
                teacherId = CInt(Session("StaffId"))
            End If

            dtClass = businessLogic.GetClassrooms(CInt(Session("CurrentCustomerId")), CInt(Session("LessonSchoolYearId")), ddlGradingPeriod.SelectedValue, iSchoolTrackId, teacherId).ObjectData

            If dtClass.Rows.Count > 0 Then
                Dim liBlank As New ListItem
                liBlank.Text = ""
                liBlank.Value = ""
                ddlClassrooms.Items.Add(liBlank)

                Dim r As DataRow
                For Each r In dtClass.Rows
                    Dim li As New ListItem
                    li.Text = r("Name").ToString
                    li.Value = r("ProductId").ToString + "," + r("ID").ToString
                    ddlClassrooms.Items.Add(li)
                Next

                'TRY SELECTING LAST SELECTED VALUE FROM THE SESSION
                Try
                    If (Session("LessonClassroomIndex") Is Nothing) Then
                        ddlClassrooms.SelectedIndex = 0
                    Else
                        If Session("LessonClassroomIndex").ToString <> "" Then
                            ddlClassrooms.SelectedIndex = Session("LessonClassroomIndex")

                            'SET CLASSROOM ASSIGNMENT ID FOR LOADING UP THE FTP and STRATEGY ASSESSMENTS LINKS
                            Dim sClassroomAssignmentId As String = "0"
                            If ddlClassrooms.SelectedValue.ToString <> "" Then
                                Dim sC() As String = Split(ddlClassrooms.SelectedValue.ToString, ",")
                                sClassroomAssignmentId = sC(1).ToString
                            End If
                            ClassroomAssignmentId.Value = sClassroomAssignmentId

                        Else
                            ddlClassrooms.SelectedIndex = 0
                        End If
                    End If
                Catch ex As Exception

                End Try

                ddlClassrooms.Enabled = True
            Else
                'THE SCHOOL HAS NOT SETUP CLASSROOMS, TEACHERS, and/or STUDENTS YET
                bIsMissingClassrooms = True
                ddlClassrooms.Enabled = False
            End If

            LoadGrid()
            SetPrintLink()
            SetUpNewLessonLink()

            If (IsReadingRoots()) Then
                lnkBtnFTPAssessment.Visible = True
            Else
                lnkBtnFTPAssessment.Visible = False
            End If

            If (IsStrategyUse()) Then
                lnkBtnStragegyAssessment.Visible = True
            Else
                lnkBtnStragegyAssessment.Visible = False
            End If

        Catch ex As Exception
            'MyError = New SFAFMemberCenterDAL.Errors
            'MyError.InsertError(ConnectionStrings("BusinessMgmt").ToString, Me.ToString + " LoadClassroomsDDL", ex.ToString, ex.StackTrace.ToString, Session("StaffID"), Session("FullName"))
            'MyError = Nothing
            Response.Redirect(AppSettings("ErrorPage").ToString)
        Finally
            dtClass = Nothing
            'oClassrooms = Nothing
        End Try
    End Sub

    Private Sub LoadDDL(ByVal data As DataTable, ByVal ddl As DropDownList, ByVal selectedValue As String)
        If data.Rows.Count > 0 Then
            Dim r As DataRow
            For Each r In data.Rows
                Dim li As New ListItem
                li.Text = r("Name")
                li.Value = r("ID")
                ddl.Items.Add(li)
            Next

            'TRY SELECTING LAST SELECTED VALUE FROM THE SESSION
            Try
                ddl.SelectedValue = selectedValue
            Catch ex As Exception

            End Try
        End If
    End Sub

#End Region

#Region "LINKS"
    Protected Sub lnkNewLesson_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkNewLesson.Click
        'IF READING WINGS 4: CHOOSE FROM AVAILABLE TITLES
        If IsReadingWings4() Then

            RadWindowTitles.VisibleOnPageLoad = True

        Else
            'CONTINUE STRAIGHT TO ADDING THE LESSON VIA CYCLE EDIT PAGE:
            Response.Redirect("CycleEdit.aspx")
            Response.End()
        End If
    End Sub

    Private Sub SetPrintLink()
        Try
            'BUILDS A DYNAMIC LINK TO RUN A REPORT
            Dim s As String = ""
            Dim sReportName As String = ""
            If IsReadingEdgeHighSchool() Or IsReadingRoots() Then
                sReportName = "ClassScoresheetsHSEdge"
            Else
                sReportName = "ClassScoresheets"
            End If

            Dim sClassroomAssignmentId As String = "0"
            If ddlClassrooms.SelectedValue.ToString <> "" Then
                Dim sC() As String = Split(ddlClassrooms.SelectedValue.ToString, ",")
                sClassroomAssignmentId = sC(1).ToString
            End If
            'BUILD PARAMETER STRING - MAKE SURE THAT EACH PARAMETER STARTS WITH A "%26" - EXCEPT FOR THE VERY FIRST PARAMETER
            s = s + "LowGradingPeriodId=2"
            s = s + "%26HighGradingPeriodId=" + Session("HighGradingPeriodId").ToString
            s = s + "%26GradingPeriodId=" + ddlGradingPeriod.SelectedValue.ToString
            s = s + "%26SchoolId=" + Session("CurrentCustomerId").ToString
            s = s + "%26SchoolYearId=" + Session("LessonSchoolYearId").ToString
            If ddlSchoolTrack.SelectedValue.ToString <> "" Then
                s = s + "%26SchoolTrackId=" + ddlSchoolTrack.SelectedValue.ToString
            End If
            s = s + "%26CustomerContactId=" + Session("CurrentCustomerId").ToString
            s = s + "%26ClassroomAssignmentId=" + sClassroomAssignmentId

            'BUILD THE FULL LINK TO THE REPORT 
            'START WITH: The report viewer page name followed by ?Rpt= (report name)
            'THEN ADD PARAMETERS: &Par=(parameter string)
            'THEN ADD RENDERING OPTIONS: &RenderFile=(PDF or leave blank for report viewer control)
            lnkPrint.NavigateUrl = "../Public/ReportViewer.aspx?Rpt=" + sReportName + "&Par=" + s + "&RenderFile=PDF"

            'IN SUMMARY 
            'THE LINK TO OPEN THE REPORT VIEWER WOULD LOOK LIKE THIS:
            'http://localhost/SFAFMemberCenterWeb/Public/ReportViewer.aspx?Rpt=ClassScoresheets&Par=LowGradingPeriodId=2%26HighGradingPeriodId=5%26GradingPeriodId=2%26SchoolId=2651%26SchoolYearId=13%26ClassroomAssignmentId=1100544&RenderFile=
            'OR
            'THE LINK TO OPEN THE REPORT DIRECTLY AS A PDF FILE WOULD LOOK LIKE THIS:
            'http://localhost/SFAFMemberCenterWeb/Public/ReportViewer.aspx?Rpt=ClassScoresheets&Par=LowGradingPeriodId=2%26HighGradingPeriodId=5%26GradingPeriodId=2%26SchoolId=2651%26SchoolYearId=13%26ClassroomAssignmentId=1100544&RenderFile=PDF
        Catch ex As Exception
            businessLogic.LogError(ex)
            Throw ex
        End Try
    End Sub

    Private Function IsReadingWings4() As Boolean
        Return IsProduct(ddlClassrooms, "CurrentReadingWings4IDs")
    End Function

    Private Function IsReadingEdgeHighSchool() As Boolean
        Return IsProduct(ddlClassrooms, "CurrentReadingEdgeHighSchoolProductIDs")
    End Function

    Private Function IsReadingRoots() As Boolean
        Return IsProduct(ddlClassrooms, "CurrentReadingRootsProductIDs")
    End Function

    Private Function IsStrategyUse() As Boolean
        Return IsProduct(ddlClassrooms, "CurrentStrategyUseProductIDs")
    End Function

    Private Function IsProduct(ByVal ddl As DropDownList, ByVal sProductKey As String) As Boolean
        Dim sP() As String = Split(ddl.SelectedValue.ToString, ",")
        Dim s As String
        Dim sProductId As String = ""
        If sP(0).ToString <> "" Then
            sProductId = sP(0).ToString
        End If

        Dim sIDs() As String = Split(AppSettings(sProductKey).ToString, ",")
        Dim bIsProduct As Boolean = False
        For Each s In sIDs
            If s.ToString = sProductId.ToString Then
                bIsProduct = True
            End If
        Next

        Return bIsProduct
    End Function
#End Region

#Region "PAGE SECURITY"
    Private Sub Lessons_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        'PAGE LEVEL SECURITY IS SET IN THE MASTER PAGE AND THEN THIS LOAD COMPLETE EVENT FIRES
        'THIS IS WHERE WE SET SECURITY ON SPECIFIC CONTROLS ON THE PAGE (AFTER WE SET THE CURRENT PAGE SECURITY LEVEL IS FOR THE USER)
        Select Case Session("CurrentPageSecurityLevel").ToString
            Case "6" 'OrgChart

            Case "5" 'Administrator

            Case "4" 'View, Add, Change, Delete

            Case "3" 'View, Add, Change

            Case "2" 'View
                lnkNewLesson.Enabled = False
            Case "1" 'Access Denied
                lnkNewLesson.Enabled = False
            Case Else 'Security is not set

        End Select

        'SINCE THE CLASSROOMS LIST AND GRID CAN BE FILTERED BASED ON SECURITY 
        '(LOAD THIS LAST AFTER THE PAGE SECURITY HAS BEEN SET IN THE MASTER PAGE)
        If Not IsPostBack() Then
            LoadClassroomsDDL()
        End If

    End Sub
#End Region

    Private Sub SetUpNewLessonLink()
        If ddlClassrooms.SelectedValue = "" Then
            lnkNewLesson.Enabled = False
        Else
            If (True) Then 'businessLogic.CheckUserObjectPermission(Session("SFAFUserName"), SALISecurityObjectTypes.Lesson.ToString(), SFAFSecurityLevel.INSERT, SFAFSecurityScope.SELF)) Then
                lnkNewLesson.Enabled = True
            Else
                lnkNewLesson.Enabled = False
            End If
        End If
    End Sub
End Class