﻿Imports Entities
Imports Npgsql.Logging
Imports pcm.DataLayer.dlLoggingNpgSQL

Public Class DebtorsMaintainenceDataLayer
    Inherits DataAccessLayerBase
    Dim RG As New Utilities.clsUtil
    Dim DebtorDataLayer As New DebtorsDataLayer

    Private _baseResponse As New BaseResponse
    Private _debtorDetailsResponse As New DebtorDetailsResponse

    Dim tmpSQL As String
    Dim ds As DataSet

    Public Function SavePersonalDetails(ByVal _DebtorsMaintainenceRequest As DebtorsMaintainenceRequest,
                                        ByVal UserName As String) As BaseResponse

        Dim NewAccNum As String

        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        Dim PersonalDetails As New DebtorsPersonalDetails
        Dim AddressDetails As New DebtorsAddressDetails
        Dim EmploymentDetails As New DebtorsEmploymentDetails
        Dim BankingDetails As New DebtorsBankingDetails
        Dim CommonDetails As New DebtorsCommonDetails
        Dim UserPermissions As New UserPermissionDetails

        PersonalDetails = _DebtorsMaintainenceRequest.PersonalDetails
        AddressDetails = _DebtorsMaintainenceRequest.AddressDetails
        EmploymentDetails = _DebtorsMaintainenceRequest.EmploymentDetails
        BankingDetails = _DebtorsMaintainenceRequest.BankingDetails
        CommonDetails = _DebtorsMaintainenceRequest.CommonDetails
        UserPermissions = _DebtorsMaintainenceRequest.UserPermissions

        '=======================================================================
        'ERROR CHECKING START
        '=======================================================================
        If PersonalDetails.IDNumber = "" Then
            _baseResponse.Message = "Please Supply An ID Number."
            _baseResponse.Success = False
            Return _baseResponse
        End If

        If PersonalDetails.FirstName = "" Then
            _baseResponse.Message = "Please Supply A First Name."
            _baseResponse.Success = False
            Return _baseResponse
        End If

        If PersonalDetails.LastName = "" Then
            _baseResponse.Message = "Please Supply A Last Name."
            _baseResponse.Success = False
            Return _baseResponse
        End If

        If PersonalDetails.Gender = "" Then
            _baseResponse.Message = "Please Supply A Sex."
            _baseResponse.Success = False
            Return _baseResponse
        End If

        If PersonalDetails.CellularNumber = "___-___-____" Then
            _baseResponse.Message = "Please Supply A Cellular Number."
            _baseResponse.Success = False
            Return _baseResponse
        End If

        If PersonalDetails.StatementDelivery = "" Then
            PersonalDetails.StatementDelivery = "POST"
        End If

        If PersonalDetails.CreditLimit = "" Then
            PersonalDetails.CreditLimit = "0"
        End If

        If PersonalDetails.ITCRating = "" Then
            PersonalDetails.ITCRating = "0"
        End If

        If EmploymentDetails.tE_8 = "" Then
            EmploymentDetails.tE_8 = "0"
        End If


        If EmploymentDetails.tE_13 = "" Then
            EmploymentDetails.tE_13 = "0"
        End If


        If UserPermissions.isLevel1 = True Then
            If Val(PersonalDetails.CreditLimit) > Val(UserPermissions.lCLimit) Then
                _baseResponse.Message = "You do not have permissions to Increase a Credit Limit."
                _baseResponse.Success = False
                Return _baseResponse
            End If
            If PersonalDetails.CurrentStatus = "ACTIVE" Then
                If UserPermissions.lStatus <> "ACTIVE" Then
                    _baseResponse.Message = "You do not have permissions to Activate an account."
                    _baseResponse.Success = False
                    Return _baseResponse
                End If
            End If
            If PersonalDetails.CurrentStatus = "DECEASED" Then
                If UserPermissions.lStatus <> "DECEASED" Then
                    _baseResponse.Message = "You do not have permissions to mark an account as DECEASED."
                    _baseResponse.Success = False
                    Return _baseResponse
                End If
            End If
            If PersonalDetails.CurrentStatus = "CLOSED" Then
                If UserPermissions.lStatus <> "CLOSED" Then
                    _baseResponse.Message = "You do not have permissions to mark an account as CLOSED."
                    _baseResponse.Success = False
                    Return _baseResponse
                End If
            End If
            If PersonalDetails.CurrentStatus = "FRAUD" Then
                If UserPermissions.lStatus <> "FRAUD" Then
                    _baseResponse.Message = "You do not have permissions to mark an account as FRAUD."
                    _baseResponse.Success = False
                    Return _baseResponse
                End If
            End If
            If PersonalDetails.CurrentStatus = "LEGAL" Then
                If UserPermissions.lStatus <> "LEGAL" Then
                    _baseResponse.Message = "You do not have permissions to mark an account as LEGAL."
                    _baseResponse.Success = False
                    Return _baseResponse
                End If
            End If
            If PersonalDetails.CurrentStatus = "PENDING" Then
                If UserPermissions.lStatus <> "PENDING" Then
                    _baseResponse.Message = "You do not have permissions to mark an account as PENDING."
                    _baseResponse.Success = False
                    Return _baseResponse
                End If
            End If
            If PersonalDetails.CurrentStatus = "SUSPENDED" Then
                If UserPermissions.lStatus <> "SUSPENDED" Then
                    _baseResponse.Message = "You do not have permissions to mark an account as SUSPENDED."
                    _baseResponse.Success = False
                    Return _baseResponse
                End If
            End If
            If PersonalDetails.CurrentStatus = "WRITE-OFF" Then
                If UserPermissions.lStatus <> "WRITE-OFF" Then
                    _baseResponse.Message = "You do not have permissions to mark an account as WRITE-OFF."
                    _baseResponse.Success = False
                    Return _baseResponse
                End If
            End If
            If PersonalDetails.CurrentStatus = "DEBT REVIEW" Then
                If UserPermissions.lStatus <> "DEBT REVIEW" Then
                    _baseResponse.Message = "You do not have permissions to mark an account as DEBT REVIEW."
                    _baseResponse.Success = False
                    Return _baseResponse
                End If
            End If
            If PersonalDetails.CurrentStatus = "REFERRAL" Then
                If UserPermissions.lStatus <> "REFERRAL" Then
                    _baseResponse.Message = "You do not have permissions to mark an account as REFERRAL."
                    _baseResponse.Success = False
                    Return _baseResponse
                End If
            End If
            If PersonalDetails.CurrentStatus = "NA" Then
                If UserPermissions.lStatus <> "NA" Then
                    _baseResponse.Message = "You do not have permissions to mark an account as NA."
                    _baseResponse.Success = False
                    Return _baseResponse
                End If
            End If
        End If

        '    If AddressDetails.cboHindi = "" Then
        '        MsgBox("Please Select A Home Type Indicator In 'ADDRESS INFORMATION'.")
        '        Exit Function
        '    End If
        '
        '    If AddressDetails.Loc = "" Then
        '        MsgBox("Please Supply A Length Of Stay At Current Address In 'ADDRESS INFORMATION'.")
        '        Exit Function
        '    End If

        '    If (AddressDetails.tA_0 = "" And AddressDetails.tA_1 = "") Then
        '        MsgBox("Please Supply A Current Residential Address In 'ADDRESS INFORMATION'.")
        '        Exit Function
        '    End If

        '    If AddressDetails.cboProv1 = "" Then
        '        MsgBox("Please Supply A Province For Current Residential Address In 'ADDRESS INFORMATION'.")
        '        Exit Function
        '    End If

        '    If AddressDetails.cboHindi = "" Then
        '        MsgBox("Please Supply A Home Type Indicator In 'ADDRESS INFORMATION'.")
        '        Exit Function
        '    End If

        If AddressDetails.cboProv2 = "" Then
            AddressDetails.cboProv2 = "NONE"
        End If

        If EmploymentDetails.tE_8 = "" Then
            _baseResponse.Message = "Please Specify An Income Total In 'EMPLOYMENT INFORMATION'."
            _baseResponse.Success = False
            Return _baseResponse
        End If

        '    If BankingDetails.cboTOA = "" Then
        '        MsgBox("Please Select A Type Of Account In 'BANKING INFORMATION'.")
        '        Exit Function
        '    End If

        '    If Len(AddressDetails.tA_18) <> 4 Then
        '        MsgBox("Please ensure a 4 digit numerical value in postal code in 'ADDRESS INFORMATION'")
        '        Exit Function
        '    End If

        If BankingDetails.cboTOA = "NONE" And PersonalDetails.BranchName <> "NONE" Then
            PersonalDetails.BranchName = "NONE"
        End If

        '    If BankingDetails.cboTOA <> "NONE" Then
        '
        '        If PersonalDetails.BranchName = "" Then
        '            MsgBox("Please Supply A Bank Name In 'BANKING INFORMATION'.")
        '            Exit Function
        '        End If
        '
        '        If BankingDetails.tB_0 = "" Then
        '            MsgBox("Please Supply A Branch Name In 'BANKING INFORMATION'.")
        '            Exit Function
        '        End If
        '
        '        If BankingDetails.tB_2 = "" Then
        '            MsgBox("Please Supply An Account Number In 'BANKING INFORMATION'.")
        '            Exit Function
        '        End If
        '
        '    End If

        If BankingDetails.cboCC1Type = "" Then
            BankingDetails.cboCC1Type = "NONE"
        End If

        If BankingDetails.cboCC2Type = "" Then
            BankingDetails.cboCC2Type = "NONE"
        End If

        If BankingDetails.cboPType = "" Then
            BankingDetails.cboPType = "IN-STORE"
        End If

        If Not RG.ValidID(PersonalDetails.IDNumber) Then
            _baseResponse.Message = "The ID number is invalid."
            _baseResponse.Success = False
            Return _baseResponse
        End If

        If Mid$(PersonalDetails.CellularNumber, 1, 2) <> "08" Then
            If Mid$(PersonalDetails.CellularNumber, 1, 2) <> "07" Then
                If Mid$(PersonalDetails.CellularNumber, 1, 2) <> "06" Then
                    _baseResponse.Message = "Please fill-in a valid Cellphone number."
                    _baseResponse.Success = False
                    Return _baseResponse
                End If
            End If
        End If

        Dim intLoop As Integer

        For intLoop = 1 To 12
            If Mid$(PersonalDetails.CellularNumber, intLoop, 1) = " " Then
                _baseResponse.Message = "Please fill-in a valid Cellphone number."
                _baseResponse.Success = False
                Return _baseResponse
            End If
        Next


        ''If Mid$(PersonalDetails.IDNumber, 1, 2) <> Mid$(PersonalDetails.DOB, 9, 2) Then
        ''    MsgBox("The inputted Date of Birth does not correspond to the inputted ID Number")
        ''    Exit Function
        ''End If

        'MM Check of ID Num
        ''If Mid$(PersonalDetails.IDNumber, 3, 2) <> Mid$(PersonalDetails.DOB, 4, 2) Then
        ''    MsgBox("The inputted Date of Birth does not correspond to the inputted ID Number")
        ''    Exit Function
        ''End If

        'dd Check of ID Num
        ''If Mid$(PersonalDetails.IDNumber, 5, 2) <> Mid$(PersonalDetails.DOB, 1, 2) Then
        ''    MsgBox("The inputted Date of Birth does not correspond to the inputted ID Number")
        ''    Exit Function
        ''End If

        If Val(Mid$(PersonalDetails.IDNumber, 7, 1)) >= 5 Then
            If PersonalDetails.Gender <> "MALE" Then
                _baseResponse.Message = "The Gender that you have selected does not match to the inputted ID Number." & vbCrLf & "Please correct before continuing."
                _baseResponse.Success = False
                Return _baseResponse
            End If
        Else
            If PersonalDetails.Gender <> "FEMALE" Then
                _baseResponse.Message = "The Gender that you have selected does not match to the inputted ID Number." & vbCrLf & "Please correct before continuing."
                _baseResponse.Success = False
                Return _baseResponse
            End If
        End If


        tmpSQL = "SELECT id_number,account_number FROM debtor_personal WHERE id_number = '" & PersonalDetails.IDNumber & "'"

        Try
            'ds = objDBWrite.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMWriteConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                If ds.Tables(0).Rows(0)("account_number") <> PersonalDetails.AccountNumber Then
                    _baseResponse.Message = "This ID number already exists for another account. Please check and re-enter it."
                    _baseResponse.Success = False
                    Return _baseResponse
                End If
            End If


            tmpSQL = "SELECT cell_number,account_number FROM debtor_personal WHERE cell_number = '" & PersonalDetails.CellularNumber & "'"
            'ds = objDBWrite.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMWriteConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                If ds.Tables(0).Rows(0)("account_number") <> PersonalDetails.AccountNumber Then
                    _baseResponse.Message = "This Cellphone number already exists for another account. Please check and re-enter it."
                    _baseResponse.Success = False
                    Return _baseResponse
                    Exit Function
                End If
            End If

        Catch ex As Exception
            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If
            Return _baseResponse
        Finally
            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If
        End Try

        '    If isRecord((Mi), "debtor_personal", "id_number", PersonalDetails.IDNumber) Then
        '        MsgBox("This ID number already exists. Please check and re-enter it."
        '        Exit Function
        '    End If
        '
        '    If Me.PersonalDetails.DontSend.value = 0 Then
        '        If isRecord((Mi), "debtor_personal", "cell_number", PersonalDetails.CellularNumber) Then
        '            MsgBox("This Cellphone number already exists in the Database. Please check and re-enter it."
        '            Exit Function
        '        End If
        '    End If
        '
        '=======================================================================
        'ERROR CHECKING END
        '=======================================================================

        Dim isUpdated As String

        If CommonDetails.Updated = True Then
            isUpdated = "true"
        Else
            isUpdated = "false"
        End If

        If PersonalDetails.CurrentStatus = "" Then
            PersonalDetails.CurrentStatus = "PENDING"
        End If

        Dim tNotes As String

        If CommonDetails.OldNotes <> "" Then
            tNotes = CommonDetails.OldNotes & vbCrLf & UserName & " " & Format(Now, "yyyy-MM-dd HH:mm:ss") & ": " & CommonDetails.NewNotes
        Else
            tNotes = CommonDetails.NewNotes
        End If

        If PersonalDetails.AccountNumber = "" Then

            'Get the new account number
            tmpSQL = "SELECT nextval('enum_debtor_account_seq') AS NewAccNum"

            Try
                'ds = objDBWrite.GetDataSet(tmpSQL)
                ds = usingObjDB.GetDataSet(_PCMWriteConnectionString, tmpSQL)
                If usingObjDB.isR(ds) Then
                    NewAccNum = ds.Tables(0).Rows(0)("NewAccNum")
                Else
                    _baseResponse.Message = "Some issue occured. Please try again."
                    _baseResponse.Success = False
                    Return _baseResponse
                End If

                'Personal Details
                tmpSQL = "INSERT INTO debtor_personal (account_number,id_number,title,first_name,middle_name,last_name,date_of_birth," &
                             "gender,home_telephone,cell_number," &
                             "email_address,next_of_kin_contact_number,send_promos,statement_delivery,card_protection,status,credit_limit," &
                             "itc_rating,auto_increase,dont_send_sms,show_on_age_analysis,is_rage_employee,debtor_notes,branch_code,preferred_language) VALUES " &
                             "('" & RG.Apos(NewAccNum) & "','" & RG.Apos(PersonalDetails.IDNumber) & "','" & RG.Apos(PersonalDetails.Title) & "','" & RG.Apos(PersonalDetails.FirstName) & "','" & RG.Apos(PersonalDetails.MiddleName) & "','" & RG.Apos(PersonalDetails.LastName) & "'," &
                             "'" & RG.Apos(PersonalDetails.DOB) & "','" & RG.Apos(PersonalDetails.Gender) & "'," &
                             "'" & RG.Apos(PersonalDetails.HomeNumber1) & "','" & RG.Apos(PersonalDetails.CellularNumber) & "'," &
                             "'" & RG.Apos(PersonalDetails.Email) & "','" & RG.Apos(PersonalDetails.ContactNumber) & "','" & RG.Apos(PersonalDetails.SendPromos) & "','" & RG.Apos(PersonalDetails.StatementDelivery) & "'," &
                             "'" & RG.Apos(PersonalDetails.CardProtection) & "','" & RG.Apos(PersonalDetails.CurrentStatus) & "'," &
                             "'" & Val(RG.Apos(PersonalDetails.CreditLimit)) & "','" & RG.Apos(PersonalDetails.ITCRating) & "','" & PersonalDetails.AutoIncrease & "'," &
                             "'" & PersonalDetails.DontSend & "','" & PersonalDetails.AgeAnalysis & "','" & EmploymentDetails.chkRageEmployee & "'," &
                             "'" & RG.Apos(tNotes) & "','" & Mid$(PersonalDetails.BranchName, 1, 3) & "','" & PersonalDetails.PreferredLanguage & "'," &
                             "'" & RG.Apos(EmploymentDetails.ClockNumber) & "')"

                'objDBWrite.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)

                'Address Details
                tmpSQL = "INSERT INTO debtor_addresses (account_number,home_type_indicator,length_previous_address,length_current_address,current_address_1," &
                         "current_address_2,current_address_3,current_address_4,current_address_province,current_address_postal_code,previous_address_1," &
                         "previous_address_2,previous_address_3,previous_address_4,previous_address_province,previous_address_postal_code,kin_address_1," &
                         "kin_address_2,kin_address_3, kin_address_4,kin_address_province,kin_address_postal_code,postal_address_1,postal_address_2," &
                         "postal_address_3,postal_address_postal_code) VALUES " &
                         "('" & RG.Apos(NewAccNum) & "','" & RG.Apos(AddressDetails.cboHindi) & "','" & RG.Apos(AddressDetails.Lop) & "','" & RG.Apos(AddressDetails.Loc) & "','" & RG.Apos(AddressDetails.tA_0) & "','" & RG.Apos(AddressDetails.tA_1) & "'," &
                         "'" & RG.Apos(AddressDetails.tA_2) & "','" & RG.Apos(AddressDetails.tA_3) & "','" & RG.Apos(AddressDetails.cboProv1) & "','" & RG.Apos(AddressDetails.tA_4) & "','" & RG.Apos(AddressDetails.tA_5) & "','" & RG.Apos(AddressDetails.tA_6) & "'," &
                         "'" & RG.Apos(AddressDetails.tA_7) & "','" & RG.Apos(AddressDetails.tA_8) & "','" & RG.Apos(AddressDetails.cboProv2) & "','" & RG.Apos(AddressDetails.tA_9) & "','" & RG.Apos(AddressDetails.tA_10) & "','" & RG.Apos(AddressDetails.tA_11) & "'," &
                         "'" & RG.Apos(AddressDetails.tA_12) & "','" & RG.Apos(AddressDetails.tA_13) & "','" & RG.Apos(AddressDetails.cboProv3) & "','" & RG.Apos(AddressDetails.tA_14) & "','" & RG.Apos(AddressDetails.tA_15) & "','" & RG.Apos(AddressDetails.tA_16) & "'," &
                         "'" & RG.Apos(AddressDetails.tA_17) & "','" & RG.Apos(AddressDetails.tA_18) & "')"

                'objDBWrite.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)

                'Employment Details
                tmpSQL = "INSERT INTO debtor_employment (account_number,employer,employer_contact_person,employer_telephone_1,employer_telephone_2,employer_fax," &
                         "employer_address_1,employer_address_2,employer_address_3,employer_address_4,employee_number,job_description,length_of_service,income_amount," &
                         "household_income,spouse_employer,spouse_employer_contact_person,spouse_employer_telephone,spouse_job_description,spouse_length_of_service," &
                         "spouse_income_amount) VALUES " &
                         "('" & RG.Apos(NewAccNum) & "','" & RG.Apos(EmploymentDetails.tE_0) & "','" & RG.Apos(EmploymentDetails.tE_1) & "','" & RG.Apos(EmploymentDetails.Work1) & "','" & RG.Apos(EmploymentDetails.Work2) & "','" & RG.Apos(EmploymentDetails.WorkFax1) & "'," &
                         "'" & RG.Apos(EmploymentDetails.tE_2) & "','" & RG.Apos(EmploymentDetails.tE_3) & "','" & RG.Apos(EmploymentDetails.tE_4) & "','" & RG.Apos(EmploymentDetails.tE_5) & "','" & RG.Apos(EmploymentDetails.tE_6) & "','" & RG.Apos(EmploymentDetails.tE_7) & "'," &
                         "'" & RG.Apos(EmploymentDetails.LOS) & "','" & RG.Apos(EmploymentDetails.tE_8) & "','" & RG.Apos(EmploymentDetails.tE_9) & "','" & RG.Apos(EmploymentDetails.tE_10) & "','" & RG.Apos(EmploymentDetails.tE_11) & "','" & RG.Apos(EmploymentDetails.Work3) & "'," &
                         "'" & RG.Apos(EmploymentDetails.tE_12) & "','" & RG.Apos(EmploymentDetails.LOS2) & "','" & RG.Apos(EmploymentDetails.tE_13) & "')"

                'objDBWrite.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)

                'Banking Details
                tmpSQL = "INSERT INTO debtor_banking (account_number,type_of_account,bank_name,branch_name,branch_number,bank_account_number,credit_card_type_1,credit_card_number_1," &
                         "credit_card_type_2,credit_card_number_2,payment_type) VALUES " &
                         "('" & RG.Apos(NewAccNum) & "','" & RG.Apos(BankingDetails.cboTOA) & "','" & RG.Apos(PersonalDetails.BranchName) & "','" & RG.Apos(BankingDetails.tB_0) & "','" & RG.Apos(BankingDetails.tB_1) & "','" & RG.Apos(BankingDetails.tB_2) & "','" & RG.Apos(BankingDetails.cboCC1Type) & "'," &
                         "'" & RG.Apos(BankingDetails.tB_3) & "','" & RG.Apos(BankingDetails.cboCC2Type) & "','" & RG.Apos(BankingDetails.tB_4) & "','" & RG.Apos(BankingDetails.cboPType) & "')"

                'objDBWrite.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)

                'Set the date of creation
                tmpSQL = "INSERT INTO debtor_dates (account_number,date_of_creation) VALUES " &
                         "('" & RG.Apos(NewAccNum) & "','" & Format(Now, "yyyy-MM-dd") & "')"

                'objDBWrite.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)

                'Create a balance
                tmpSQL = "INSERT INTO financial_balances (account_number,credit_limit,total,original_credit_limit) VALUES " &
                         "('" & NewAccNum & "','" & Val(PersonalDetails.CreditLimit) & "','0','" & Val(PersonalDetails.CreditLimit) & "')"

                'objDBWrite.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
            Catch ex As Exception
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
                _baseResponse.Message = "Some issue occured. Please try again."
                _baseResponse.Success = False
                Return _baseResponse
            Finally
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
            End Try


            _baseResponse.Message = "Account created successfully."
            _baseResponse.Success = True
            Return _baseResponse

        ElseIf Val(PersonalDetails.AccountNumber) > 0 Then
            Try
                tmpSQL = "UPDATE debtor_personal SET id_number = '" & RG.Apos(PersonalDetails.IDNumber) & "',title = '" & RG.Apos(PersonalDetails.Title) & "'," &
                            "first_name = '" & RG.Apos(PersonalDetails.FirstName) & "',middle_name = '" & RG.Apos(PersonalDetails.MiddleName) & "',last_name = '" & RG.Apos(PersonalDetails.LastName) & "'," &
                            "gender = '" & RG.Apos(PersonalDetails.Gender) & "', home_telephone = '" & RG.Apos(PersonalDetails.HomeNumber1) & "'," &
                            "cell_number = '" & RG.Apos(PersonalDetails.CellularNumber) & "'," &
                            "email_address = '" & RG.Apos(PersonalDetails.Email) & "',next_of_kin_contact_number = '" & RG.Apos(PersonalDetails.ContactNumber) & "'," &
                            "send_promos = '" & RG.Apos(PersonalDetails.SendPromos) & "',statement_delivery = '" & RG.Apos(PersonalDetails.StatementDelivery) & "',card_protection = '" & RG.Apos(PersonalDetails.CardProtection) & "'," &
                            "status = '" & RG.Apos(PersonalDetails.CurrentStatus) & "',credit_limit = '" & Val(RG.Apos(PersonalDetails.CreditLimit)) & "'," &
                            "itc_rating = '" & RG.Apos(PersonalDetails.ITCRating) & "',auto_increase = '" & PersonalDetails.AutoIncrease & "'," &
                            "dont_send_sms = '" & PersonalDetails.DontSend & "',is_rage_employee = '" & EmploymentDetails.chkRageEmployee & "'," &
                            "debtor_notes = '" & RG.Apos(tNotes) & "'," &
                            "clock_number = '" & RG.Apos(EmploymentDetails.ClockNumber) & "'," &
                            "is_updated = '" & CommonDetails.Updated & "',call_thin_files = '" & CommonDetails.ThinFileCall & "',show_on_age_analysis = '" & PersonalDetails.AgeAnalysis & "',branch_code = '" & Mid$(PersonalDetails.BranchName, 1, 3) & "', " &
                            "preferred_language = '" & PersonalDetails.PreferredLanguage & "' WHERE account_number = '" & PersonalDetails.AccountNumber & "'"

                'objDBWrite.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)

                'Check whether the records were actually inserted. Picking up problems where the record existed
                'in the first table but not the rest

                'Address Details
                tmpSQL = "SELECT * FROM debtor_addresses WHERE account_number = '" & PersonalDetails.AccountNumber & "'"

                'ds = objDBWrite.GetDataSet(tmpSQL)
                ds = usingObjDB.GetDataSet(_PCMWriteConnectionString, tmpSQL)
                If objDBWrite.isR(ds) Then
                    tmpSQL = "UPDATE debtor_addresses SET home_type_indicator = '" & RG.Apos(AddressDetails.cboHindi) & "',length_previous_address = '" & RG.Apos(AddressDetails.Lop) & "'," &
                            "length_current_address = '" & RG.Apos(AddressDetails.Loc) & "',current_address_1 = '" & RG.Apos(AddressDetails.tA_0) & "',current_address_2 = '" & RG.Apos(AddressDetails.tA_1) & "'," &
                            "current_address_3 = '" & RG.Apos(AddressDetails.tA_2) & "',current_address_4 = '" & RG.Apos(AddressDetails.tA_3) & "',current_address_province = '" & RG.Apos(AddressDetails.cboProv1) & "'," &
                            "current_address_postal_code = '" & RG.Apos(AddressDetails.tA_4) & "',previous_address_1 = '" & RG.Apos(AddressDetails.tA_5) & "',previous_address_2 = '" & RG.Apos(AddressDetails.tA_6) & "'," &
                            "previous_address_3 = '" & RG.Apos(AddressDetails.tA_7) & "',previous_address_4 = '" & RG.Apos(AddressDetails.tA_8) & "',previous_address_province = '" & RG.Apos(AddressDetails.cboProv2) & "'," &
                            "previous_address_postal_code = '" & RG.Apos(AddressDetails.tA_9) & "',kin_address_1 = '" & RG.Apos(AddressDetails.tA_10) & "',kin_address_2 = '" & RG.Apos(AddressDetails.tA_11) & "'," &
                            "kin_address_3 = '" & RG.Apos(AddressDetails.tA_12) & "',kin_address_4 = '" & RG.Apos(AddressDetails.tA_13) & "',kin_address_postal_code = '" & RG.Apos(AddressDetails.tA_14) & "'," &
                            "postal_address_1 = '" & RG.Apos(AddressDetails.tA_15) & "',postal_address_2 = '" & RG.Apos(AddressDetails.tA_16) & "',postal_address_3 = '" & RG.Apos(AddressDetails.tA_17) & "'," &
                            "postal_address_postal_code = '" & RG.Apos(AddressDetails.tA_18) & "',kin_address_province = '" & RG.Apos(AddressDetails.cboProv3) & "' WHERE account_number = '" & PersonalDetails.AccountNumber & "'"
                Else
                    tmpSQL = "INSERT INTO debtor_addresses (account_number,home_type_indicator,length_previous_address,length_current_address,current_address_1," &
                             "current_address_2,current_address_3,current_address_4,current_address_province,current_address_postal_code,previous_address_1," &
                             "previous_address_2,previous_address_3,previous_address_4,previous_address_province,previous_address_postal_code,kin_address_1," &
                             "kin_address_2,kin_address_3, kin_address_4,kin_address_province,kin_address_postal_code,postal_address_1,postal_address_2," &
                             "postal_address_3,postal_address_postal_code) VALUES " &
                             "('" & PersonalDetails.AccountNumber & "','" & RG.Apos(AddressDetails.cboHindi) & "','" & RG.Apos(AddressDetails.Lop) & "','" & RG.Apos(AddressDetails.Loc) & "','" & RG.Apos(AddressDetails.tA_0) & "','" & RG.Apos(AddressDetails.tA_1) & "'," &
                             "'" & RG.Apos(AddressDetails.tA_2) & "','" & RG.Apos(AddressDetails.tA_3) & "','" & RG.Apos(AddressDetails.cboProv1) & "','" & RG.Apos(AddressDetails.tA_4) & "','" & RG.Apos(AddressDetails.tA_5) & "','" & RG.Apos(AddressDetails.tA_6) & "'," &
                             "'" & RG.Apos(AddressDetails.tA_7) & "','" & RG.Apos(AddressDetails.tA_8) & "','" & RG.Apos(AddressDetails.cboProv2) & "','" & RG.Apos(AddressDetails.tA_9) & "','" & RG.Apos(AddressDetails.tA_10) & "','" & RG.Apos(AddressDetails.tA_11) & "'," &
                             "'" & RG.Apos(AddressDetails.tA_12) & "','" & RG.Apos(AddressDetails.tA_13) & "','" & RG.Apos(AddressDetails.cboProv3) & "','" & RG.Apos(AddressDetails.tA_14) & "','" & RG.Apos(AddressDetails.tA_15) & "','" & RG.Apos(AddressDetails.tA_16) & "'," &
                             "'" & RG.Apos(AddressDetails.tA_17) & "','" & RG.Apos(AddressDetails.tA_18) & "')"
                End If

                'objDBWrite.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)

                'Employment Details
                tmpSQL = "SELECT * FROM debtor_employment WHERE account_number = '" & PersonalDetails.AccountNumber & "'"

                'ds = objDBWrite.GetDataSet(tmpSQL)
                ds = usingObjDB.GetDataSet(_PCMWriteConnectionString, tmpSQL)
                If usingObjDB.isR(ds) Then
                    tmpSQL = "UPDATE debtor_employment SET employer = '" & RG.Apos(EmploymentDetails.tE_0) & "',employer_contact_person = '" & RG.Apos(EmploymentDetails.tE_1) & "'," &
                             "employer_telephone_1 = '" & RG.Apos(EmploymentDetails.Work1) & "',employer_telephone_2 = '" & RG.Apos(EmploymentDetails.Work2) & "',employer_fax = '" & RG.Apos(EmploymentDetails.WorkFax1) & "'," &
                             "employer_address_1 = '" & RG.Apos(EmploymentDetails.tE_2) & "',employer_address_2 = '" & RG.Apos(EmploymentDetails.tE_3) & "',employer_address_3 = '" & RG.Apos(EmploymentDetails.tE_4) & "'," &
                             "employer_address_4 = '" & RG.Apos(EmploymentDetails.tE_5) & "',employee_number = '" & RG.Apos(EmploymentDetails.tE_6) & "',job_description = '" & RG.Apos(EmploymentDetails.tE_7) & "'," &
                             "length_of_service = '" & RG.Apos(EmploymentDetails.LOS) & "',income_amount = '" & RG.Apos(EmploymentDetails.tE_8) & "',household_income = '" & RG.Apos(EmploymentDetails.tE_9) & "'," &
                             "spouse_employer =  '" & RG.Apos(EmploymentDetails.tE_10) & "',spouse_employer_contact_person = '" & RG.Apos(EmploymentDetails.tE_11) & "'," &
                             "spouse_employer_telephone = '" & RG.Apos(EmploymentDetails.Work3) & "',spouse_job_description =  '" & RG.Apos(EmploymentDetails.tE_12) & "'," &
                             "spouse_length_of_service = '" & RG.Apos(EmploymentDetails.LOS2) & "',spouse_income_amount =  '" & RG.Apos(EmploymentDetails.tE_13) & "' WHERE account_number = '" & PersonalDetails.AccountNumber & "'"
                Else
                    tmpSQL = "INSERT INTO debtor_employment (account_number,employer,employer_contact_person,employer_telephone_1,employer_telephone_2,employer_fax," &
                             "employer_address_1,employer_address_2,employer_address_3,employer_address_4,employee_number,job_description,length_of_service,income_amount," &
                             "household_income,spouse_employer,spouse_employer_contact_person,spouse_employer_telephone,spouse_job_description,spouse_length_of_service," &
                             "spouse_income_amount) VALUES " &
                             "('" & PersonalDetails.AccountNumber & "','" & RG.Apos(EmploymentDetails.tE_0) & "','" & RG.Apos(EmploymentDetails.tE_1) & "','" & RG.Apos(EmploymentDetails.Work1) & "','" & RG.Apos(EmploymentDetails.Work2) & "','" & RG.Apos(EmploymentDetails.WorkFax1) & "'," &
                             "'" & RG.Apos(EmploymentDetails.tE_2) & "','" & RG.Apos(EmploymentDetails.tE_3) & "','" & RG.Apos(EmploymentDetails.tE_4) & "','" & RG.Apos(EmploymentDetails.tE_5) & "','" & RG.Apos(EmploymentDetails.tE_6) & "','" & RG.Apos(EmploymentDetails.tE_7) & "'," &
                             "'" & RG.Apos(EmploymentDetails.LOS) & "','" & RG.Apos(EmploymentDetails.tE_8) & "','" & RG.Apos(EmploymentDetails.tE_9) & "','" & RG.Apos(EmploymentDetails.tE_10) & "','" & RG.Apos(EmploymentDetails.tE_11) & "','" & RG.Apos(EmploymentDetails.Work3) & "'," &
                             "'" & RG.Apos(EmploymentDetails.tE_12) & "','" & RG.Apos(EmploymentDetails.LOS2) & "','" & RG.Apos(EmploymentDetails.tE_13) & "')"
                End If

                'objDBWrite.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)

                'Banking Details
                tmpSQL = "SELECT * FROM debtor_banking WHERE account_number = '" & PersonalDetails.AccountNumber & "'"

                'ds = objDBWrite.GetDataSet(tmpSQL)
                ds = usingObjDB.GetDataSet(_PCMWriteConnectionString, tmpSQL)
                If usingObjDB.isR(ds) Then
                    tmpSQL = "UPDATE debtor_banking SET type_of_account = '" & RG.Apos(BankingDetails.cboTOA) & "',bank_name = '" & RG.Apos(PersonalDetails.BranchName) & "'," &
                             "branch_name = '" & RG.Apos(BankingDetails.tB_0) & "',bank_account_number = '" & RG.Apos(BankingDetails.tB_2) & "',credit_card_type_1 = '" & RG.Apos(BankingDetails.cboCC1Type) & "'," &
                             "credit_card_number_1 = '" & RG.Apos(BankingDetails.tB_3) & "',credit_card_type_2 = '" & RG.Apos(BankingDetails.cboCC2Type) & "',credit_card_number_2 = '" & RG.Apos(BankingDetails.tB_4) & "'," &
                             "branch_number = '" & RG.Apos(BankingDetails.tB_2) & "',payment_type = '" & RG.Apos(BankingDetails.cboPType) & "' WHERE account_number = '" & PersonalDetails.AccountNumber & "'"
                Else
                    tmpSQL = "INSERT INTO debtor_banking (account_number,type_of_account,bank_name,branch_name,branch_number,bank_account_number,credit_card_type_1,credit_card_number_1," &
                             "credit_card_type_2,credit_card_number_2,payment_type) VALUES " &
                             "('" & PersonalDetails.AccountNumber & "','" & RG.Apos(BankingDetails.cboTOA) & "','" & RG.Apos(PersonalDetails.BranchName) & "','" & RG.Apos(BankingDetails.tB_0) & "','" & RG.Apos(BankingDetails.tB_1) & "','" & RG.Apos(BankingDetails.tB_2) & "','" & RG.Apos(BankingDetails.cboCC1Type) & "'," &
                             "'" & RG.Apos(BankingDetails.tB_3) & "','" & RG.Apos(BankingDetails.cboCC2Type) & "','" & RG.Apos(BankingDetails.tB_4) & "','" & RG.Apos(BankingDetails.cboPType) & "')"
                End If

                'objDBWrite.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
                'Dates
                If UserPermissions.lStatus <> PersonalDetails.CurrentStatus Then
                    If PersonalDetails.CurrentStatus = "WRITE-OFF" Or PersonalDetails.CurrentStatus = "LEGAL" Then
                        tmpSQL = "SELECT * FROM debtor_dates WHERE account_number = '" & PersonalDetails.AccountNumber & "'"

                        'ds = objDBWrite.GetDataSet(tmpSQL)
                        ds = usingObjDB.GetDataSet(_PCMWriteConnectionString, tmpSQL)
                        If usingObjDB.isR(ds) Then
                            tmpSQL = "UPDATE debtor_dates SET date_of_default = '" & Format(Now, "yyyy-MM-dd") & "' WHERE account_number = '" & PersonalDetails.AccountNumber & "'"
                        Else
                            tmpSQL = "INSERT INTO debtor_dates (account_number,date_of_creation) VALUES " &
                                         "('" & PersonalDetails.AccountNumber & "','" & Format(Now, "yyyy-MM-dd") & "')"
                        End If

                        'objDBWrite.ExecuteQuery(tmpSQL)
                        usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
                    End If
                End If

                tmpSQL = "SELECT * FROM financial_balances WHERE account_number ='" & PersonalDetails.AccountNumber & "'"
                'ds = objDBWrite.GetDataSet(tmpSQL)
                ds = usingObjDB.GetDataSet(_PCMWriteConnectionString, tmpSQL)
                If Not usingObjDB.isR(ds) Then
                    'Create a balance
                    tmpSQL = "INSERT INTO financial_balances (account_number,total,credit_limit,original_credit_limit) VALUES " &
                             "('" & PersonalDetails.AccountNumber & "',0," & Val(PersonalDetails.CreditLimit) & "," & Val(PersonalDetails.CreditLimit) & ")"
                Else
                    tmpSQL = "UPDATE financial_balances SET credit_limit = " & Val(PersonalDetails.CreditLimit) & " WHERE account_number = '" & PersonalDetails.AccountNumber & "'"
                End If

                'objDBWrite.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)

                If PersonalDetails.CurrentStatus <> UserPermissions.lStatus Then
                    AddDebLog(PersonalDetails.AccountNumber, "Account Status Changed", UserPermissions.lStatus, PersonalDetails.CurrentStatus, UserName)
                End If

                'Dim new_num As String
                'Dim tmpMessage As String

                'If UserPermissions.lStatus = "DECLINED" And PersonalDetails.CurrentStatus = "ACTIVE" Then
                '    If MsgBox("Do you want to send an SMS to the Customer to inform them that their Account has been Approved?", vbYesNo) = vbYes Then

                '        'txtNum = rS(Mi & 42).Fields("PersonalDetails.CellularNumber") & ""
                '        new_num = Mid$(PersonalDetails.CellularNumber, 1, 3) & Mid$(PersonalDetails.CellularNumber, 5, 3) & Mid$(PersonalDetails.CellularNumber, 9, 4)

                '        tmpMessage = ". We have reviewed your Rage application and your Rage account has now been approved! Please bring your ID to any Rage store to collect your card."

                '        ''Send_INET_SMS Mid$(new_num, 2, 9), "Hi " & Mid$(ProperCase(PersonalDetails.FirstName), 1, 10) & tmpMessage

                '        tmpSQL = "INSERT INTO sms_sent (sms_date,sms_time,account_number,message) VALUES " &
                '                     "('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "hh:mm:ss") & "','" & PersonalDetails.AccountNumber & "','" & RG.Apos(tmpMessage) & "')"

                '        objDBWrite.ExecuteQuery(tmpSQL)
                '    End If
                'End If

                'If UserPermissions.lStatus = "REFERRAL" And PersonalDetails.CurrentStatus = "ACTIVE" Then
                '    If MsgBox("Do you want to send an SMS to the Customer to inform them that their Account has been Approved?", vbYesNo) = vbYes Then

                '        'txtNum = rS(Mi & 42).Fields("PersonalDetails.CellularNumber") & ""
                '        new_num = Mid$(PersonalDetails.CellularNumber, 1, 3) & Mid$(PersonalDetails.CellularNumber, 5, 3) & Mid$(PersonalDetails.CellularNumber, 9, 4)

                '        tmpMessage = ". We have reviewed your Rage application and your Rage account has now been approved! Please bring your ID to any Rage store to collect your card."

                '        ''Send_INET_SMS Mid$(new_num, 2, 9), "Hi " & Mid$(ProperCase(PersonalDetails.FirstName), 1, 10) & tmpMessage

                '        tmpSQL = "INSERT INTO sms_sent (sms_date,sms_time,account_number,message) VALUES " &
                '                     "('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "hh:mm:ss") & "','" & PersonalDetails.AccountNumber & "','" & RG.Apos(tmpMessage) & "')"
                '        objDBWrite.ExecuteQuery(tmpSQL)
                '    End If
                'End If

                'If UserPermissions.lStatus = "DECLINED" And PersonalDetails.CurrentStatus = "REFERRAL" Then
                '    If MsgBox("Do you want to send an SMS to the Customer to inform them to bring their ID and 3 months bank statements to a Rage store?", vbYesNo) = vbYes Then


                '        'txtNum = rS(Mi & 42).Fields("PersonalDetails.CellularNumber") & ""
                '        new_num = Mid$(PersonalDetails.CellularNumber, 1, 3) & Mid$(PersonalDetails.CellularNumber, 5, 3) & Mid$(PersonalDetails.CellularNumber, 9, 4)

                '        tmpMessage = ". We need some further info before we can approve your Rage Account. Please bring your ID and 3 months Bank Statements to any Rage store."

                '        ''Send_INET_SMS Mid$(new_num, 2, 9), "Hi " & Mid$(ProperCase(PersonalDetails.FirstName), 1, 10) & tmpMessage

                '        tmpSQL = "INSERT INTO sms_sent (sms_date,sms_time,account_number,message) VALUES " &
                '                     "('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "hh:mm:ss") & "','" & PersonalDetails.AccountNumber & "','" & RG.Apos(tmpMessage) & "')"
                '        objDBWrite.ExecuteQuery(tmpSQL)
                '    End If
                'End If

                'If UserPermissions.lStatus = "REFERRAL" And PersonalDetails.CurrentStatus = "DECLINED" Then
                '    If MsgBox("Do you want to send an SMS to the Customer to inform them that their account was Declined", vbYesNo) = vbYes Then

                '        'txtNum = rS(Mi & 42).Fields("PersonalDetails.CellularNumber") & ""
                '        new_num = Mid$(PersonalDetails.CellularNumber, 1, 3) & Mid$(PersonalDetails.CellularNumber, 5, 3) & Mid$(PersonalDetails.CellularNumber, 9, 4)

                '        ''Send_INET_SMS Mid$(new_num, 2, 9), "We are sorry to inform you that your Rage account has been declined. Please re-apply after 2 months and come visit again soon. Better luck next time!"

                '        tmpSQL = "INSERT INTO sms_sent (sms_date,sms_time,account_number,message) VALUES " &
                '                     "('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "hh:mm:ss") & "','" & PersonalDetails.AccountNumber & "','" & RG.Apos(tmpMessage) & "')"

                '        objDBWrite.ExecuteQuery(tmpSQL)
                '    End If
                'End If

            Catch ex As Exception
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
                Return Nothing
            Finally
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
            End Try

            If PersonalDetails.CreditLimit <> UserPermissions.lCLimit Then
                AddDebLog(PersonalDetails.AccountNumber, "Account Limit Changed", UserPermissions.lCLimit, PersonalDetails.CreditLimit, UserName)
            End If

            Dim BranchC As String
            BranchC = Mid$(PersonalDetails.BranchName, 1, 3)

            'If UserPermissions.lThinFileCall = "True" Then
            '    If CommonDetails.ThinFileCall = True Then
            '        AddDebLog(PersonalDetails.AccountNumber, "Thin File Call Updated", "True", "False")
            '    End If
            'End If

            'If UserPermissions.lThinFileCall = "False" Then
            '    If CommonDetails.ThinFileCall = True Then
            '        AddDebLog(PersonalDetails.AccountNumber, "Thin File Call Updated", "False", "True")
            '    End If
            'End If

            If UserPermissions.lBranchCode <> BranchC Then
                AddDebLog(PersonalDetails.AccountNumber, "Branch Code Changed", UserPermissions.lBranchCode, BranchC, UserName)
            End If

            If UserPermissions.lLanguage <> PersonalDetails.PreferredLanguage Then
                AddDebLog(PersonalDetails.AccountNumber, "Preferred Language Changed", UserPermissions.lLanguage, PersonalDetails.PreferredLanguage, UserName)
            End If

            If UserPermissions.lIsUpdated <> CommonDetails.Updated Then
                AddDebLog(PersonalDetails.AccountNumber, "Is Updated Changed", UserPermissions.lIsUpdated, CommonDetails.Updated, UserName)
            End If

            ''****************************************************************************

            _baseResponse.Message = "Account updated successfully."
            _baseResponse.Success = True
            Return _baseResponse

        End If

    End Function

    Public Function GetBranchList(Optional ByVal BranchCode As String = "") As DataSet

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        If BranchCode <> "" Then
            tmpSQL = "SELECT * FROM branch_details WHERE branch_code = '" & BranchCode & "' ORDER BY branch_name"
        Else
            tmpSQL = "Select * FROM branch_details ORDER BY branch_name"
        End If


        Try
            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                Return ds
            Else
                Return Nothing
            End If
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return Nothing
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try
    End Function

    Public Function GetReportingList(AccountNumber As String) As DataSet

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        If AccountNumber <> "" Then
            tmpSQL = "Select * From debtor_change_log WHERE account_number = '" & AccountNumber & "' ORDER BY change_date,change_time ASC"

            Try
                'ds = objDB.GetDataSet(tmpSQL)
                ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
                If usingObjDB.isR(ds) Then
                    Return ds
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                'If (objDB IsNot Nothing) Then
                '    objDB.CloseConnection()
                'End If
                Return Nothing
            Finally
                'If (objDB IsNot Nothing) Then
                '    objDB.CloseConnection()
                'End If
            End Try
        Else
            Return Nothing
        End If

    End Function

    Public Sub AddDebLog(ByVal tAccNum As String, ByVal tDescript As String,
                         ByVal tOldVal As String, ByVal tNewVal As String, ByVal UserName As String)

        tmpSQL = "INSERT INTO debtor_change_log (change_date,change_time,username,account_number,description,old_value,new_value) " &
                 "VALUES ('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','" & RG.Apos(UserName) & "','" & tAccNum & "'," &
                 "'" & RG.Apos(tDescript) & "','" & RG.Apos(tOldVal) & "','" & RG.Apos(tNewVal) & "')"

        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        'objDBWrite.ExecuteQuery(tmpSQL)
        usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)

        'If (objDBWrite IsNot Nothing) Then
        '    objDBWrite.CloseConnection()
        'End If

    End Sub

    Public Function ValidID(IdNumber As String)
        Dim ValidIDResponse As Boolean

        'Save Debtor Personal Details
        ValidIDResponse = RG.ValidID(IdNumber)

        Return ValidIDResponse
    End Function

    Public Function GetDebtors(Field As String, Criteria As String, IsRageEmployeesOnly As Boolean, ByVal IsLast100 As Boolean) As DataTable
        Dim GetDebtorsResponse As DataTable

        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        If Field = "" Then

            _baseResponse.Message = "Please specify a Search Field."
            _baseResponse.Success = False
            Return Nothing
        End If

        If Criteria = "" Then
            If Field = "ACCOUNT NUMBER" Then
                Criteria = "1"
            Else
                _baseResponse.Message = "Please enter a search criteria."
                _baseResponse.Success = False
                Return Nothing
            End If
        End If

        If Field = "ACCOUNT NUMBER" Then
            tmpSQL = "Select debtor_personal.account_number,debtor_personal.id_number,debtor_personal.first_name,debtor_personal.last_name," &
                     "debtor_personal.status,debtor_personal.cell_number,card_details.card_number As cardnum FROM debtor_personal " &
                     "LEFT OUTER JOIN card_details On card_details.account_number = debtor_personal.account_number " &
                     "WHERE debtor_personal.account_number Like '" & Criteria & "%' "

            If IsRageEmployeesOnly = True Then
                tmpSQL = tmpSQL & "AND debtor_personal.is_rage_employee = '1' ORDER BY debtor_personal.account_number"
            End If

        ElseIf Field = "ID NUMBER" Then
            tmpSQL = "SELECT debtor_personal.account_number,debtor_personal.id_number,debtor_personal.first_name,debtor_personal.last_name," &
                     "debtor_personal.status,debtor_personal.cell_number,card_details.card_number AS cardnum FROM debtor_personal " &
                     "LEFT OUTER JOIN card_details ON card_details.account_number = debtor_personal.account_number " &
                     "WHERE debtor_personal.id_number LIKE '" & Criteria & "%' "

            If IsRageEmployeesOnly = True Then
                tmpSQL = tmpSQL & "AND debtor_personal.is_rage_employee = '1' ORDER BY debtor_personal.account_number"
            End If


        ElseIf Field = "LAST NAME" Then

            tmpSQL = "SELECT debtor_personal.account_number,debtor_personal.id_number,debtor_personal.first_name,debtor_personal.last_name," &
                     "debtor_personal.status,debtor_personal.cell_number,card_details.card_number AS cardnum FROM debtor_personal " &
                     "LEFT OUTER JOIN card_details ON card_details.account_number = debtor_personal.account_number " &
                     "WHERE debtor_personal.last_name ILIKE '" & Criteria & "%' "


            If IsRageEmployeesOnly = True Then
                tmpSQL = tmpSQL & "AND debtor_personal.is_rage_employee = '1' ORDER BY debtor_personal.account_number"
            End If

        ElseIf Field = "CELLPHONE" Then

            tmpSQL = "SELECT debtor_personal.is_bad_debt_warning,debtor_personal.is_updated,debtor_personal.account_number,debtor_personal.id_number,debtor_personal.first_name,debtor_personal.last_name," &
                     "debtor_personal.status,debtor_personal.cell_number,card_details.card_number AS cardnum FROM debtor_personal " &
                     "LEFT OUTER JOIN card_details ON card_details.account_number = debtor_personal.account_number " &
                     "WHERE debtor_personal.cell_number LIKE '" & Criteria & "%' "

            If IsRageEmployeesOnly = True Then
                tmpSQL = tmpSQL & "AND debtor_personal.is_rage_employee = '1' ORDER BY debtor_personal.account_number "
            End If
        End If

        If IsRageEmployeesOnly = False Then
            If IsLast100 = True Then
                tmpSQL = tmpSQL & "ORDER BY debtor_personal.account_number DESC LIMIT 100"
            Else
                tmpSQL = tmpSQL & "ORDER BY debtor_personal.account_number LIMIT 100"
            End If
        End If

        'If Not GetRS Then Exit Sub
        'lvItems.ListItems.Clear

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If objDBRead.isR(ds) Then
                GetDebtorsResponse = ds.Tables(0)
            Else
                Return Nothing
            End If

        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
        End Try

        Return GetDebtorsResponse
    End Function

    Public Function GetSelectedDebtorsDetails(strAccNum As String) As DebtorDetailsResponse
        If Val(strAccNum) > 0 Then
            Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPCMRead")

            tmpSQL = "SELECT debtor_addresses.*,debtor_banking.*,debtor_employment.*,debtor_personal.*,debtor_rage_filters.*," &
                 "debtor_dates.*,debtor_cpa_accounts.*,debtor_cpa_enquiry.*,debtor_defaults.*," &
                 "debtor_judgements.*,debtor_nlr_accounts.*,debtor_nlr_enquiry.*," &
                 "debtor_not_approved.*,debtor_sbcfilters.*,card_dates.date_last_used,card_details.card_number AS cardnum," &
                 "debtor_policyfilters.*,debtor_empericarules.*,debtor_xml.xml_data,financial_balances.credit_limit AS creditlimit " &
                 "From debtor_personal " &
                 "Left Outer Join debtor_banking ON debtor_personal.account_number = debtor_banking.account_number " &
                 "Left Outer Join debtor_employment ON debtor_personal.account_number = debtor_employment.account_number " &
                 "Left Outer Join debtor_addresses ON debtor_personal.account_number = debtor_addresses.account_number " &
                 "Left Outer Join debtor_dates ON debtor_personal.account_number = debtor_dates.account_number " &
                 "Left Outer Join debtor_cpa_accounts ON debtor_personal.account_number = debtor_cpa_accounts.account_number " &
                 "Left Outer Join debtor_cpa_enquiry ON debtor_personal.account_number = debtor_cpa_enquiry.account_number " &
                 "Left Outer Join debtor_defaults ON debtor_personal.account_number = debtor_defaults.account_number " &
                 "Left Outer Join debtor_judgements ON debtor_personal.account_number = debtor_judgements.account_number " &
                 "Left Outer Join debtor_nlr_accounts ON debtor_personal.account_number = debtor_nlr_accounts.account_number " &
                 "Left Outer Join debtor_nlr_enquiry ON debtor_personal.account_number = debtor_nlr_enquiry.account_number " &
                 "Left Outer Join debtor_not_approved ON debtor_personal.account_number = debtor_not_approved.account_number " &
                 "Left Outer Join debtor_sbcfilters ON debtor_personal.account_number = debtor_sbcfilters.account_number " &
                 "Left Outer Join debtor_policyfilters ON debtor_personal.account_number = debtor_policyfilters.account_number " &
                 "Left Outer Join debtor_empericarules ON debtor_personal.account_number = debtor_empericarules.account_number " &
                 "Left Outer Join debtor_xml ON debtor_personal.account_number = debtor_xml.account_number " &
                 "Left Outer Join financial_balances ON debtor_personal.account_number = financial_balances.account_number " &
                 "Left Outer Join card_details ON debtor_personal.account_number = card_details.account_number " &
                 "Left Outer Join card_dates ON card_details.card_number = card_dates.card_number " &
                 "Left Outer Join debtor_rage_filters ON debtor_personal.account_number = debtor_rage_filters.account_number WHERE debtor_personal.account_number = '" & strAccNum & "'"

            Try
                ds = objDBRead.GetDataSet(tmpSQL)
                If objDBRead.isR(ds) Then
                    _debtorDetailsResponse.GetSelectedDebtorsResponse = ds.Tables(0)
                    _debtorDetailsResponse.Success = True
                Else
                    _debtorDetailsResponse.Message = "Some issue occured. Please try again."
                    _debtorDetailsResponse.Success = False
                    Return _debtorDetailsResponse
                End If

            Catch ex As Exception
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                _debtorDetailsResponse.Message = "Some issue occured. Please try again."
                _debtorDetailsResponse.Success = False
                Return _debtorDetailsResponse
            Finally
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
            End Try
        Else

            _debtorDetailsResponse.Message = "Account Number is missing"
            _debtorDetailsResponse.Success = False
            Return _debtorDetailsResponse
        End If

        Return _debtorDetailsResponse
    End Function

    Public Function GetDebtorXmlData(AccountNumber As String) As String
        Dim XMLData As String

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        If AccountNumber <> "" Then
            tmpSQL = "Select * From debtor_xml WHERE account_number = '" & AccountNumber & "'"

            Try
                ds = objDB.GetDataSet(tmpSQL)
                If objDB.isR(ds) Then
                    XMLData = ds.Tables(0).Rows(0)("xml_data").ToString
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                If (objDB IsNot Nothing) Then
                    objDB.CloseConnection()
                End If
                Return Nothing
            Finally
                If (objDB IsNot Nothing) Then
                    objDB.CloseConnection()
                End If
            End Try

            Return XMLData
        Else
            Return Nothing
        End If
        Return Nothing
    End Function

    Public Function GetDebtorsReportingData(ByVal AccountNumber As String) As DebtorsReporting

        Dim _NewDebtor As New DebtorsReporting

        Dim _NewAgeAnalysis As New Debtor_AgeAnalysis
        Dim _NewAgeAnalysisList As New List(Of Debtor_AgeAnalysis)

        Dim _NewPaymentPlan As New List(Of Debtor_PaymentPlan)
        Dim _NewTransactions As New List(Of Transactions)
        Dim _NewClosingBalances As New List(Of Debtor_ClosingBalances)
        Dim _NewContactHistory As New List(Of Debtor_ContactHistory)
        Dim _NewLastTransactions As New Debtor_LastTransactions
        Dim _NewChanges As New List(Of DebtorChangeLog)

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        tmpSQL = "SELECT dp.account_number,dp.status,dp.first_name,dp.itc_rating,dp.last_name,dp.cell_number,dp.preferred_language," &
                 "fb.credit_limit,fb.total,fb.current_balance,fb.p30,fb.p60," &
                 "fb.p90,fb.p120,fb.p150,fb.current_contact_level,fb.next_contact_time,dp.home_telephone,dp.home_telephone_2,dp.fax_number," &
                 "dp.spouse_cell_number,dp.next_of_kin,dp.next_of_kin_contact_number,dp.send_promos,de.employer_telephone_1 " &
                 "FROM debtor_personal dp " &
                 "INNER JOIN financial_balances fb ON dp.account_number = fb.account_number " &
                 "INNER JOIN debtor_employment de ON dp.account_number = de.account_number " &
                 "WHERE dp.account_number = '" & AccountNumber & "'"

        Try
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    _NewAgeAnalysis.aaTotal = dr("total") & ""
                    _NewAgeAnalysis.aaCurrent = dr("current_balance") & ""
                    _NewAgeAnalysis.aa30Days = dr("p30") & ""
                    _NewAgeAnalysis.aa60Days = dr("p60") & ""
                    _NewAgeAnalysis.aa90Days = dr("p90") & ""
                    _NewAgeAnalysis.aa120Days = dr("p120") & ""
                    _NewAgeAnalysis.aa150Days = dr("p150") & ""
                    _NewAgeAnalysisList.Add(_NewAgeAnalysis)

                Next
                _NewPaymentPlan = DebtorDataLayer.GetPaymentsPlans(AccountNumber)
                _NewTransactions = DebtorDataLayer.GetTransactions(AccountNumber)
                _NewClosingBalances = DebtorDataLayer.GetClosingBalances(AccountNumber)
                _NewContactHistory = DebtorDataLayer.GetContactHistory(AccountNumber)
                _NewLastTransactions = DebtorDataLayer.GetLastTransactions(AccountNumber)
                _NewChanges = DebtorDataLayer.GetChanges(AccountNumber)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try

        _NewDebtor.AgeAnalysis = _NewAgeAnalysisList
        _NewDebtor.PaymentPlans = _NewPaymentPlan
        _NewDebtor.Transactions = _NewTransactions
        _NewDebtor.ClosingBalances = _NewClosingBalances
        _NewDebtor.ContactHistory = _NewContactHistory
        _NewDebtor.ChangeHistory = _NewChanges

        Return _NewDebtor

    End Function
End Class
