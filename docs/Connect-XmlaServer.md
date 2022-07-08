---
external help file: PsXmla.dll-Help.xml
Module Name: PsXmla
online version:
schema: 2.0.0
---

# Connect-XmlaServer

## SYNOPSIS
{{ Fill in the Synopsis }}

## SYNTAX

### ConnectionString
```
Connect-XmlaServer [-ConnectionString] <String> [-AccessToken <String>] [-Username <String>]
 [-Password <SecureString>] [-Credential <PSCredential>] [-ClientId <String>] [-InteractiveAuthentication]
 [<CommonParameters>]
```

### Properties
```
Connect-XmlaServer [-DataSource] <String> [[-InitialCatalog] <String>] [-AccessToken <String>]
 [-Username <String>] [-Password <SecureString>] [-Credential <PSCredential>] [-ClientId <String>]
 [-InteractiveAuthentication] [<CommonParameters>]
```

### PowerBi
```
Connect-XmlaServer [-PowerBiWorkspace] <String> [[-PowerBiDataset] <String>] [[-PowerBiTenant] <String>]
 [-AccessToken <String>] [-Username <String>] [-Password <SecureString>] [-Credential <PSCredential>]
 [-ClientId <String>] [-InteractiveAuthentication] [<CommonParameters>]
```

## DESCRIPTION
{{ Fill in the Description }}

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -AccessToken
{{ Fill AccessToken Description }}

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ClientId
{{ Fill ClientId Description }}

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ConnectionString
{{ Fill ConnectionString Description }}

```yaml
Type: String
Parameter Sets: ConnectionString
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -Credential
{{ Fill Credential Description }}

```yaml
Type: PSCredential
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DataSource
{{ Fill DataSource Description }}

```yaml
Type: String
Parameter Sets: Properties
Aliases: Server, ServerName, ServerInstance

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -InitialCatalog
{{ Fill InitialCatalog Description }}

```yaml
Type: String
Parameter Sets: Properties
Aliases: Database, DatabaseName

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -InteractiveAuthentication
{{ Fill InteractiveAuthentication Description }}

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Password
{{ Fill Password Description }}

```yaml
Type: SecureString
Parameter Sets: (All)
Aliases: ClientSecret

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PowerBiDataset
{{ Fill PowerBiDataset Description }}

```yaml
Type: String
Parameter Sets: PowerBi
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -PowerBiTenant
{{ Fill PowerBiTenant Description }}

```yaml
Type: String
Parameter Sets: PowerBi
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -PowerBiWorkspace
{{ Fill PowerBiWorkspace Description }}

```yaml
Type: String
Parameter Sets: PowerBi
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Username
{{ Fill Username Description }}

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String
## OUTPUTS

### Microsoft.AnalysisServices.Tabular.Server
## NOTES

## RELATED LINKS
