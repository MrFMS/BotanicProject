
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
    private string currentString = axiom;
    StringBuilder sb = new StringBuilder();
    public int iterations = 5;
    public float length = 2f;
    public float angle = 30f;
    List<Dictionary<char, string>> rules = new List<Dictionary<char, string>>();
    private Stack<StackTransform> transformStack = new Stack<StackTransform>();

    public StateTree state = StateTree.GREEN;
    public Material[] materials;
    public Material CurrentMaterial;
    public List<GameObject> Branches = new List<GameObject>();
    public List<GameObject> Leaves = new List<GameObject>();
    public GameObject BranchPrefab;
    public GameObject Leaf;

    public float saveRadius;


    private Dictionary<char, string> rule;

   // private Dictionary<char, string> rules;


    // Start is called before the first frame update
    void Start()
    {
        CurrentMaterial = materials[0];
        rules.Add(new Dictionary<char, string>
        {
            // "F[+F][-F]"
            { 'X', "[F[-X+F[+FX]][*-X+F[+FX]][/-X+F[+FX]-X]]" },
            { 'F', "FF" }
        });

        rules.Add(new Dictionary<char, string>
        {
            { 'X', "[F-[*X+X]+F[/+FX]-X]" },
            { 'F', "FF" }
        });


        rules.Add(new Dictionary<char, string>
        {
            { 'X', "[-FX][/+FX][*FX]" },
            { 'F', "FF" }
        });

        rules.Add(new Dictionary<char, string>
        {
            { 'X', "[-FX]X[/+FX][*+F-FX]" },
            { 'F', "FF" }
        });

        rule = rules[Random.Range(0, rules.Count - 1)];
        GenerateOneIteration();
  
    }

    public void GenerateOneIteration()
    {
        foreach (char c in currentString)
        {
            sb.Append(rule.ContainsKey(c) ? rule[c] : c.ToString());
        }
        currentString = sb.ToString();
        sb = new StringBuilder();
        Generate();
        StopCoroutine("ThirstyTree");
        StartCoroutine("ThirstyTree");
    }

    void Generate()
    {
        foreach (var c in currentString)
        {
            switch (c)
            {
                case 'F':
                    GameObject branch = Instantiate(BranchPrefab);
                    branch.GetComponent<MeshRenderer>().material = CurrentMaterial;

                    //branch.GetComponent<Branch>().material = CurrentMaterial;
                    // branch.GetComponent<Branch>().GenerateBranch(2f);
                    /* if (Branches.Count == 0)
                     {
                         branch.GetComponent<Branch>().GenerateBranch(2f);
                     }
                     else
                     {
                         branch.GetComponent<Branch>().GenerateBranch(saveRadius);
                     }*/
                    // saveRadius = branch.GetComponent<Branch>().radiusUp;
                    branch.transform.SetPositionAndRotation(transform.position, transform.rotation); //GetComponent<LineRenderer>().SetPosition(0, transform.position);
                    transform.Translate(Vector3.up);
                    branch.GetComponent<BrancheCylindre>().tree = this;
                    //branch.GetComponent<Branch>().tree = this;
                    Branches.Add(branch);
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
                    GameObject leaf = Instantiate(Leaf, transform.position, transform.rotation);
                    leaf.GetComponent<Leaf>().tree = this;
                    Leaves.Add(leaf);
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
                    break;
            }
        }
    }

    IEnumerator ThirstyTree()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);
            switch (state)
            {
                case StateTree.GREEN:
                    state = StateTree.ORANGE;
                    ChangeMaterial(materials[1]);
                    break;
                case StateTree.ORANGE:
                    state = StateTree.BROWN;
                   ChangeMaterial(materials[2]);
                    break;
                case StateTree.BROWN:
                    state = StateTree.BLACK;
                    ChangeMaterial(materials[3]);
                    break;
                case StateTree.BLACK:
                    TreeController.Instance.DestroyTree(gameObject);
                    break;
                default:
                    state = StateTree.GREEN;
                    break;
            }
            yield return null;
        }
    }


    private void OnDestroy()
    {
        foreach (var branch in Branches)
        {
            Destroy(branch);
        }
        foreach (var leaf in Leaves)
        {
            Destroy(leaf);
        }
    }

    void ChangeMaterial(Material  material)
    {
        CurrentMaterial = material;
        foreach (var branch in Branches)
        {
            branch.GetComponent<MeshRenderer>().material = CurrentMaterial;
            //branch.GetComponent<Branch>().material = material;
        }
    }

}
