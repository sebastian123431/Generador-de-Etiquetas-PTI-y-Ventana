# Copilot Instructions

## Project Guidelines
- When the Windows Forms designer fails to load with CodeDomDesignerLoader errors, check for emojis or special Unicode characters in control properties like Text, as they can cause serialization issues.
- Ensure that existing data is not deleted when making modifications.
- Confirm that the check for `SelectedIndex` is applied on line 126 of `frm_generadorVentana_Citricos.cs`, validating `Items.Count` before assigning `SelectedIndex`.