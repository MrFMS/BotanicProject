using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class StackTransform
{
    public Vector3 position;
    public Quaternion rotation;
}

public class Tree : MonoBehaviour
{

    private const string axiom = "X";
    private string currentString;
    public int iterations = 5;
    public float length = 2f;
    public float angle = 30f;
    private Stack<StackTransform> transformStack = new Stack<StackTransform>();

    public GameObject BranchPrefab;


    private Dictionary<char, string> rules;


    // Start is called before the first frame update
    void Start()
    {

        rules = new Dictionary<char, string>
        {
            // "F[+F][-F]"
            { 'X', "[F[-X+F[+FX]][*-X+F[+FX]][/-X+F[+FX]-X]]" },
            { 'F', "FF" }
        };

        Generate();
    }

    void Generate()
    {

        currentString =axiom;
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < iterations; i++)
        {
            foreach (char c in currentString)
            {
                sb.Append(rules.ContainsKey(c) ? rules[c] : c.ToString());
            }

            currentString = sb.ToString();
            sb = new StringBuilder();
        }
        foreach (var c in currentString)
        {
            switch (c)
            {
                case 'F':

                    GameObject branch = Instantiate(BranchPrefab);

                   // branch.GetComponent<Branch>().GenerateBranch();
                    branch.transform.SetPositionAndRotation(transform.position, transform.rotation); //GetComponent<LineRenderer>().SetPosition(0, transform.position);
                    transform.Translate(Vector3.up);
                   // branch.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                    break;
                case 'X':
                    break;
                case '+':
                    transform.Rotate(Vector3.back * angle);
                    break;
                case '-':
                    transform.Rotate(Vector3.forward * angle);
                    break;
                case '[':
                    transformStack.Push(new StackTransform()
                    {
                        position = transform.position,
                        rotation = transform.rotation
                    });
                    break;
                case ']':
                    StackTransform ti = transformStack.Pop();
                    transform.position = ti.position;
                    transform.rotation = ti.rotation;
                    break;
                case '*':
                    transform.Rotate(Vector3.up * 120);
                    break;

                case '/':
                    transform.Rotate(Vector3.down * 120);
                    break;
                default:
                    throw new InvalidOperationException("Invalid L-tree operation");
                    break;
            }
        }
    }



}
