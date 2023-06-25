using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class TransparentWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    static extern int SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwflags);

    private const int GWL_STYLE = -20;
    private const uint WS_POPUP = 0x00080000;
    private const uint WS_VISIBLE = 0x00000020;
    private const int HWND_TOPMOST = -1;
    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOMOVE = 0x0002;
    private const uint LWA_COLORKEY = 0x00000001;

    private IntPtr hwnd;
    private bool mouseOverUI = false;
    //private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);


    private void Start()
    {
#if !UNITY_EDITOR
        hwnd = GetActiveWindow();

        var margins = new MARGINS() { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea(hwnd, ref margins);

        SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
        SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, 0);
#endif

        Application.runInBackground = true;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(ray, Mathf.Infinity);
        SetClickThrough(hit);

    }

    private void SetClickThrough(bool collider)
    {
        if (collider || mouseOverUI)
        {
            SetWindowLong(hwnd, GWL_STYLE, WS_POPUP );
            Debug.Log("Click through");
            //SetLayeredWindowAttributes(hwnd, 0, 0, LWA_COLORKEY);
        }
        else
        {
            SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
            Debug.Log("Not click through");
            //SetLayeredWindowAttributes(hwnd, 0, 255, LWA_COLORKEY);
        }
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 1.0f;
        //Debug.Log("VEC : " + vec);
        return vec; 
    }
        
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f));
        return worldPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOverUI = true;
        Debug.Log("Mouse enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOverUI = false;
        Debug.Log("Mouse exit");
    }

}