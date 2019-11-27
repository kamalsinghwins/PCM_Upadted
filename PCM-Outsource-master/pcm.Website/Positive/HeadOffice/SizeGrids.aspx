<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Intranet/Intranet.Master" CodeBehind="SizeGrids.aspx.vb" Inherits="pcm.Website.SizeGrids" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxwgv" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <style type="text/css">
        .date_panel
        {}
    </style>

    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SideHolder" runat="server">
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainHolder" runat="server">
    <script type="text/javascript">
        var index = -1;

        function grid_RowClick(s, e) {
            if (grid.IsEditing() == true) {
                index = e.visibleIndex;

                s.UpdateEdit();
            }
            else {
                s.SetFocusedRowIndex(e.visibleIndex); // for better visual appearence                                
                s.StartEditRow(e.visibleIndex);
            }
        }

        function grid_EndCallback(s, e) {
            if (index != -1) {
                var _index = index;
                index = -1;

                s.SetFocusedRowIndex(_index); // for better visual appearence

                s.StartEditRow(_index);
            }
        }
    </script>

     <div>
            <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                Width="564px">
                <SettingsBehavior AllowFocusedRow="True" />
                <SettingsEditing Mode="Inline" />
                <Columns>
                    <dxwgv:GridViewDataTextColumn FieldName="Oid" ReadOnly="True" Visible="False" VisibleIndex="1"
                        SortOrder="Ascending" SortIndex="0">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="Name" VisibleIndex="0" Width="200px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="Price" VisibleIndex="1" Width="150px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataDateColumn FieldName="Announced" VisibleIndex="2" Width="200px">
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataCheckColumn FieldName="Discontinued" VisibleIndex="3" Width="100px">
                    </dxwgv:GridViewDataCheckColumn>
                </Columns>
                <ClientSideEvents RowDblClick="grid_RowClick" RowClick="grid_RowClick" EndCallback="grid_EndCallback" />
            </dxwgv:ASPxGridView>
           
        </div>
</asp:Content>
