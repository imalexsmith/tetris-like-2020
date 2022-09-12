using UnityEngine;

// ========================
// Revision 2019.11.05
// ========================

public class EditorGUIPainter
{
    // ========================================================================================
    private bool _hasError;
    public bool HasError
    {
        get { return _hasError; }
        set
        {
            if (value)
                _startRepaintTime = Time.realtimeSinceStartup;
            else
                _errorMessage = "";
            _hasError = value;
        }
    }

    private string _errorMessage;
    public string ErrorMessage
    {
        get { return _errorMessage; }
        set
        {
            HasError = !string.IsNullOrEmpty(value);
            _errorMessage = value;
        }
    }

    private bool _hasWarning;
    public bool HasWarning
    {
        get { return _hasWarning; }
        set
        {
            if (_hasError) return;

            if (value)
                _startRepaintTime = Time.realtimeSinceStartup;
            else
                _warningMessage = "";

            _hasWarning = value;
        }
    }

    private string _warningMessage;
    public string WarningMessage
    {
        get { return _warningMessage; }
        set
        {
            if (!_hasError)
            {
                HasWarning = !string.IsNullOrEmpty(value);
                _warningMessage = value;
            }
        }
    }

    private bool _hasAcceptance;
    public bool HasAcceptance
    {
        get { return _hasAcceptance; }
        set
        {
            if (_hasError || _hasWarning) return;

            if (value)
                _startRepaintTime = Time.realtimeSinceStartup;
            else
                _acceptanceMessage = "";

            _hasAcceptance = value;
        }
    }

    private string _acceptanceMessage;
    public string AcceptanceMessage
    {
        get { return _acceptanceMessage; }
        set
        {
            if (!_hasError && !_hasWarning)
            {
                HasAcceptance = !string.IsNullOrEmpty(value);
                _acceptanceMessage = value;
            }
        }
    }

    public float TimeToRepaint = 2F;

    private readonly Color _defaultColor;
    private float _startRepaintTime;


    // ========================================================================================
    public EditorGUIPainter()
    {
        _defaultColor = GUI.color;
    }

    public void Begin<T>(CustomEditorBase<T> editor) where T : Object
    {
        if (_hasError || _hasWarning || _hasAcceptance)
        {
            var delta = Time.realtimeSinceStartup - _startRepaintTime;
            float r = 1F, g = 1F, b = 1F;
            var smoothColor = Mathf.Clamp(delta / TimeToRepaint, 0F, 1F);

            if (_hasError)
            {
                if (delta < TimeToRepaint)
                {
                    g = smoothColor;
                    b = smoothColor;

                }
                else HasError = false;

                editor.ErrorMessage += _hasError ? _errorMessage: "";
            }
            else if (_hasWarning)
            {
                if (delta < TimeToRepaint)
                {
                    b = smoothColor;
                }
                else HasWarning = false;

                editor.WarningMessage += _hasWarning ? _warningMessage: "";
            }
            else if (_hasAcceptance)
            {
                if (delta < TimeToRepaint)
                {
                    r = smoothColor;
                    b = smoothColor;
                }
                else HasAcceptance = false;

                editor.InformationMessage += _hasAcceptance ? AcceptanceMessage: "";
            }

            GUI.color = new Color(r, g, b);
            editor.Repaint();
        }
    }

    public void End()
    {
        GUI.color = _defaultColor;
    }
}