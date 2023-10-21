
using Cashier.Components.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;

namespace Cashier.Components.Pages
{
    public partial class SidebarDemo : ComponentBase
    {
        [Inject]
        private IDialogService? MyDialogService { get; set; }

        private IDialogReference? _dialog;

        private readonly SimplePerson simplePerson = new()
        {
            Firstname = "Steve",
            Lastname = "Roth",
            Age = 42,
        };

        // protected override async Task OnInitializedAsync()
        // {
        //     // Your initialization code goes here
        // }

        // protected override async Task OnAfterRenderAsync(bool firstRender)
        // {
        //     if (firstRender)
        //     {
        //         // @* await BaseLayerLuminance.SetValueFor() = StandardLuminance.DarkMode; *@
        //     }
        // }

        private async Task ShowPanel()
        {
            Console.WriteLine("yo!");

            _dialog = await MyDialogService.ShowPanelAsync<SimplePanel>(simplePerson, new DialogParameters<SimplePerson>()
            {
                Content = simplePerson,
                Alignment = HorizontalAlignment.Left,
                Title = "Hello!",
                PrimaryAction = "Yes",
                SecondaryAction = "No",
            });
            DialogResult result = await _dialog.Result;
            HandlePanel(result);
        }

        private async Task OpenPanelLeftAsync()
        {
            // DemoLogger.WriteLine($"Open left panel");
            DialogParameters<SimplePerson> parameters = new()
            {
                Content = simplePerson,
                Title = $"Hello {simplePerson.Firstname}",
                Alignment = HorizontalAlignment.Left,
                Modal = false,
                ShowDismiss = false,
                PrimaryAction = "Maybe",
                SecondaryAction = "Cancel",
                Width = "500px",
            };
            _dialog = await MyDialogService.ShowPanelAsync<SimplePanel>(simplePerson, parameters);
            DialogResult result = await _dialog.Result;
            HandlePanel(result);
        }

        private async Task OpenPanelRightAsync()
        {
            // DemoLogger.WriteLine($"Open right panel");

            _dialog = await MyDialogService.ShowPanelAsync<SimplePanel>(simplePerson, new DialogParameters<SimplePerson>()
            {
                Content = simplePerson,
                Alignment = HorizontalAlignment.Right,
                Title = $"Hello {simplePerson.Firstname}",
                PrimaryAction = "Yes",
                SecondaryAction = "No",
            });
            DialogResult result = await _dialog.Result;
            HandlePanel(result);
        }

        private static void HandlePanel(DialogResult result)
        {
            if (result.Cancelled)
            {
                // DemoLogger.WriteLine($"Panel cancelled");
                return;
            }

            if (result.Data is not null)
            {
                SimplePerson? simplePerson = result.Data as SimplePerson;
                // DemoLogger.WriteLine($"Panel closed by {simplePerson?.Firstname} {simplePerson?.Lastname} ({simplePerson?.Age})");
                return;
            }
        }
    }
}