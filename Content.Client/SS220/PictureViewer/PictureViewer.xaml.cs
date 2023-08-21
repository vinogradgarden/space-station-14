using System.Numerics;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Input;
using Robust.Shared.Utility;

namespace Content.Client.SS220.PictureViewer;

[GenerateTypedNameReferences]
public sealed partial class PictureViewer : Control
{
    private ResPath _viewedPicture;

    private const float ScrollSensitivity = 0.1f;
    private const float MaxZoom = 10f;

    private float _zoom = 1f;
    private bool _draggin = false;
    private Vector2 _offset = Vector2.Zero;

    public ResPath ViewedPicture
    {
        get => _viewedPicture;
        set
        {
            _viewedPicture = value;
            Picture.TexturePath = value.ToString();
            NoImageLabel.Visible = false;
        }
    }

    private void UpdateZoom()
    {
        var invZoom = 1 / _zoom;
        Picture.TextureScale = new Vector2(invZoom, invZoom);
    }

    private void UpdateOffset()
    {
        //Picture.Margin = new Thickness(_offset.X, _offset.Y);
        var position = _offset;
        var rect = UIBox2.FromDimensions(position, this.Size);
        Picture.Arrange(rect);
    }

    protected override void MouseWheel(GUIMouseWheelEventArgs args)
    {
        base.MouseWheel(args);
        _zoom -= args.Delta.Y * ScrollSensitivity;
        _zoom = float.Clamp(_zoom, 1, MaxZoom);
        //ogger.DebugS("ZOOM: ", _zoom.ToString());
        UpdateZoom();
        UpdateOffset();

        args.Handle();
    }

    protected override void KeyBindDown(GUIBoundKeyEventArgs args)
    {
        base.KeyBindDown(args);

        if (args.Function == EngineKeyFunctions.Use)
        {
            _draggin = true;
        }
    }

    protected override void KeyBindUp(GUIBoundKeyEventArgs args)
    {
        base.KeyBindUp(args);

        if (args.Function == EngineKeyFunctions.Use)
        {
            _draggin = false;
        }
    }

    protected override void MouseMove(GUIMouseMoveEventArgs args)
    {
        base.MouseMove(args);

        if (!_draggin)
            return;

        //_recentering = false;
        _offset += new Vector2(args.Relative.X, args.Relative.Y);
        //Logger.DebugS("OFFSET: ", _offset.ToString());
        UpdateOffset();

        /*
        if (_offset != Vector2.Zero)
        {
            _recenter.Disabled = false;
        }
        else
        {
            _recenter.Disabled = true;
        }
        */
    }

    public PictureViewer()
    {
        RobustXamlLoader.Load(this);
    }
}
