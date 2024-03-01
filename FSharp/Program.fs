// For more information see https://aka.ms/fsharp-console-apps
printfn "Hello from F#"

type BinaryTree<'a> =
    | Leaf
    | Node of 'a * BinaryTree<'a> * BinaryTree<'a>

// 中序遍历函数
let rec inorderTraversal tree =
    match tree with
    | Leaf -> []
    | Node (value, left, right) ->
        inorderTraversal left @ [value] @ inorderTraversal right

// 示例用法
let exampleTree =
    Node (1,
        Node (2, Leaf, Leaf),
        Node (3,
            Node (4, Leaf, Leaf),
            Leaf
        )
    )

let result = inorderTraversal exampleTree
printfn "中序遍历结果：%A" result
