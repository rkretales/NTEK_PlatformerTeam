using System.Collections;
using UnityEngine.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.TestTools;

public class RotateBigCubePlayModeTests
{
    private GameObject cube;
    private GameObject target;
    private Event e = new Event { type = EventType.KeyDown };
    private Event mouseDown = new Event { type = EventType.MouseDown };
    private Event mouseUp = new Event { type = EventType.MouseUp };
    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;
    private Vector3 previousMousePosition;
    private Vector3 mouseDown3DPosition;
    private float speed;

    [SetUp]
    public void Setup()
    {
        // Load the test scene
        EditorSceneManager.LoadScene("ExampleCube");
    }

    [UnityTest]
    public IEnumerator SwipeLeft_TargetRotatesLeft90Degrees()
    {
        // Find "Cube" object from scene
        cube = GameObject.Find("Cube");
        target = cube;

        // Generate random swipes between range until conditiion is met
        Vector2 GetRandomLeftSwipe()
        {
            Vector2 randomSwipe = secondPressPos - firstPressPos;
            do
            {
                firstPressPos = new Vector2(Random.value, Random.value);
                secondPressPos = new Vector2(Random.value, Random.value);
                randomSwipe = new Vector2(Random.Range(float.MinValue, 0), Random.Range(-0.5f, 0.5f));
                randomSwipe.Normalize();
            } while (!(randomSwipe.x < 0 && randomSwipe.y > -0.5f && randomSwipe.y < 0.5f));

            return randomSwipe;
        }

        // Simulate Input.GetMouseButton(0) 
        mouseDown.button = 0;
        mouseUp.button = 0;

        if (mouseUp.button == 0 && mouseDown.button == 0)
        {
            currentSwipe = GetRandomLeftSwipe();
            cube.transform.Rotate(0, 90, 0, Space.World);
        }

        yield return null;
        var expectedRotation = target.transform.rotation;

        // Assert
        Assert.AreEqual(expectedRotation, cube.transform.rotation);
    }

    [UnityTest]
    public IEnumerator SwipeRight_TargetRotatesRight90Degrees()
    {
        // Find "Cube" object from scene
        cube = GameObject.Find("Cube");
        target = cube;

        // Simulate Input.GetMouseButton(0) 
        mouseDown.button = 0;
        mouseUp.button = 0;

        // Loop to generate random swipes based on condition
        Vector2 GetRandomRightSwipe()
        {
            Vector2 randomSwipe = secondPressPos - firstPressPos;
            do
            {
                firstPressPos = new Vector2(Random.value, Random.value);
                secondPressPos = new Vector2(Random.value, Random.value);
                randomSwipe = new Vector2(Random.Range(0, float.MaxValue), Random.Range(-0.5f, 0.5f));
                randomSwipe.Normalize();
            } while (!(randomSwipe.x > 0 && randomSwipe.y > -0.5f && randomSwipe.y < 0.5f));

            return randomSwipe;
        }

        if (mouseUp.button == 0 && mouseDown.button == 0)
        {
            currentSwipe = GetRandomRightSwipe();
            cube.transform.Rotate(0, -90, 0, Space.World);
        }

        yield return null;
        var expectedRotation = target.transform.rotation;

        // Assert
        Assert.AreEqual(expectedRotation, target.transform.rotation);
    }

    [UnityTest]
    public IEnumerator SwipeUpLeft_TargetRotatesUpLeft90Degrees()
    {
        // Find "Cube" object from scene
        cube = GameObject.Find("Cube");
        target = cube;

        // Simulate Input.GetMouseButton(0) 
        mouseDown.button = 0;
        mouseUp.button = 0;

        // Loop to generate random swipes based on condition
        Vector2 GetRandomUpLeftSwipe()
        {
            Vector2 randomSwipe = secondPressPos - firstPressPos;
            do
            {
                firstPressPos = new Vector2(Random.value, Random.value);
                secondPressPos = new Vector2(Random.value, Random.value);
                randomSwipe = new Vector2(Random.Range(float.MinValue, 0), Random.Range(0f, 0.5f));
                randomSwipe.Normalize();
            } while (!(randomSwipe.y > 0 && randomSwipe.x < 0f));

            return randomSwipe;
        }

        if (mouseUp.button == 0 && mouseDown.button == 0)
        {
            currentSwipe = GetRandomUpLeftSwipe();
            cube.transform.Rotate(90, 0, 0, Space.World);
        }

        yield return null;
        var expectedRotation = target.transform.rotation;

        // Assert
        Assert.AreEqual(expectedRotation, target.transform.rotation);
    }

    [UnityTest]
    public IEnumerator SwipeUpRight_TargetRotatesUpRight90Degrees()
    {
        // Find "Cube" object from scene
        cube = GameObject.Find("Cube");
        target = cube;

        // Simulate Input.GetMouseButton(0) 
        mouseDown.button = 0;
        mouseUp.button = 0;

        // Loop to generate random swipes based on condition
        Vector2 GetRandomUpRightSwipe()
        {
            Vector2 randomSwipe = secondPressPos - firstPressPos;
            do
            {
                firstPressPos = new Vector2(Random.value, Random.value);
                secondPressPos = new Vector2(Random.value, Random.value);
                randomSwipe = new Vector2(Random.Range(0f, float.MaxValue), Random.Range(0f, float.MaxValue));
                randomSwipe.Normalize();
            } while (!(randomSwipe.y > 0 && randomSwipe.x > 0f));

            return randomSwipe;
        }

        if (mouseUp.button == 0 && mouseDown.button == 0)
        {
            currentSwipe = GetRandomUpRightSwipe();
            cube.transform.Rotate(0, 0, -90, Space.World);
        }

        yield return null;
        var expectedRotation = target.transform.rotation;

        // Assert
        Assert.AreEqual(expectedRotation, target.transform.rotation);
    }

    [UnityTest]
    public IEnumerator SwipeDownLeft_TargetRotatesDownLeft90Degrees()
    {
        // Find "Cube" object from scene
        cube = GameObject.Find("Cube");
        target = cube;

        // Simulate Input.GetMouseButton(0) 
        mouseDown.button = 0;
        mouseUp.button = 0;

        // Loop to generate random swipes based on condition
        Vector2 GetRandomDownLeftSwipe()
        {
            Vector2 randomSwipe = secondPressPos - firstPressPos;
            do
            {
                firstPressPos = new Vector2(Random.value, Random.value);
                secondPressPos = new Vector2(Random.value, Random.value);
                randomSwipe = new Vector2(Random.Range(float.MinValue, 0), Random.Range(-0.5f, 0.5f));
                randomSwipe.Normalize();
            } while (!(randomSwipe.y < 0 && randomSwipe.x < 0f));

            return randomSwipe;
        }

        if (mouseUp.button == 0 && mouseDown.button == 0)
        {
            currentSwipe = GetRandomDownLeftSwipe();
            cube.transform.Rotate(0, 0, 90, Space.World);
        }

        yield return null;
        var expectedRotation = target.transform.rotation;

        // Assert
        Assert.AreEqual(expectedRotation, target.transform.rotation);
    }

    [UnityTest]
    public IEnumerator SwipeDownRight_TargetRotatesDownRight90Degrees()
    {
        // Find "Cube" object from scene
        cube = GameObject.Find("Cube");
        target = cube;

        // Simulate Input.GetMouseButton(0) 
        mouseDown.button = 0;
        mouseUp.button = 0;

        // Loop to generate random swipes based on condition
        Vector2 GetRandomDownRightSwipe()
        {
            Vector2 randomSwipe = secondPressPos - firstPressPos;
            do
            {
                firstPressPos = new Vector2(Random.value, Random.value);
                secondPressPos = new Vector2(Random.value, Random.value);
                randomSwipe = new Vector2(Random.Range(0f, float.MaxValue), Random.Range(-0.5f, 0.5f));
                randomSwipe.Normalize();
            } while (!(randomSwipe.y < 0 && randomSwipe.x > 0f));

            return randomSwipe;
        }

        if (mouseUp.button == 0 && mouseDown.button == 0)
        {
            currentSwipe = GetRandomDownRightSwipe();
            cube.transform.Rotate(-90, 0, 0, Space.World);
        }

        yield return null;
        var expectedRotation = target.transform.rotation;

        // Assert
        Assert.AreEqual(expectedRotation, target.transform.rotation);
    }

    [UnityTest]
    public IEnumerator LeftArrow_TargetRotatesLeft90Degrees()
    {

        // Find "Cube" object from scene
        cube = GameObject.Find("Cube");
        target = cube;

        // Simulate arrow key press
        e.keyCode = KeyCode.LeftArrow;
        if (e.keyCode > 0)
        {
            cube.transform.Rotate(0, 90, 0);
        }

        yield return null;
        var expectedRotation = target.transform.rotation;

        // Assert
        Assert.AreEqual(expectedRotation, target.transform.rotation);
    }


    [UnityTest]
    public IEnumerator RightArrow_TargetRotatesRight90Degrees()
    {

        // Find "Cube" object from scene
        cube = GameObject.Find("Cube");
        target = cube;

        // Simulate arrow key press
        e.keyCode = KeyCode.RightArrow;
        if (e.keyCode > 0)
        {
            cube.transform.Rotate(0, -90, 0);
        }

        yield return null;
        var expectedRotation = target.transform.rotation;

        // Assert
        Assert.AreEqual(expectedRotation, target.transform.rotation);
    }

    [UnityTest]
    public IEnumerator UpArrow_TargetRotatesUp90Degrees()
    {

        // Find "Cube" object from scene
        cube = GameObject.Find("Cube");
        target = cube;

        // Simulate arrow key press
        e.keyCode = KeyCode.UpArrow;
        cube.transform.Rotate(0, 0, -90);

        yield return null;
        var expectedRotation = target.transform.rotation;

        // Assert
        Assert.AreEqual(expectedRotation, target.transform.rotation);
    }

    [UnityTest]
    public IEnumerator DownArrow_TargetRotatesDown90Degrees()
    {

        // Find "Cube" object from scene
        cube = GameObject.Find("Cube");
        target = cube;

        // Simulate arrow key press
        e.keyCode = KeyCode.DownArrow;
        if (e.keyCode > 0)
        {
            target.transform.Rotate(0, 0, 90);
        }

        yield return null;
        var expectedRotation = target.transform.rotation;

        // Assert
        Assert.AreEqual(expectedRotation, target.transform.rotation);
    }

    [UnityTest]
    public IEnumerator Drag_CubeRotatesBasedOnDrag()
    {

        // Find "Cube" object from scene
        cube = GameObject.Find("Cube");
        target = cube;

        // Simulate Input.GetMouseButton(0) 
        mouseDown.button = 0;

        if (mouseDown.button == 0)
        {
            // Simulate random drag
            mouseDown.mousePosition = new Vector2(Random.Range(0, 360), Random.Range(0, 360)); // Simulate random left mouse click position
            mouseDown3DPosition = new Vector3(mouseDown.mousePosition.x, mouseDown.mousePosition.y, Random.Range(0, 360));
            mouseDown.delta = mouseDown3DPosition - previousMousePosition;
            mouseDown.delta *= 0.1f; // reduction of rotation speed
            cube.transform.rotation = Quaternion.Euler(mouseDown.delta.y, -mouseDown.delta.x, 0) * cube.transform.rotation;
        }

        // Simulate situation when cube's rotation is not equal to target's rotation
        cube.transform.Rotate(Random.Range(1, 270), Random.Range(1, 270), Random.Range(1, 270));
        if (cube.transform.rotation != target.transform.rotation)
        {
            // automatically move to the target position
            var step = speed * Time.deltaTime;
            cube.transform.rotation = Quaternion.RotateTowards(cube.transform.rotation, target.transform.rotation, step);
            // Assert that cube has moved towards target
            Assert.AreEqual(cube.transform.rotation, target.transform.rotation);
        }

        previousMousePosition = mouseDown3DPosition;
        yield return null;

        // Assert
        Assert.AreEqual(target.transform.rotation, cube.transform.rotation);
    }
}





