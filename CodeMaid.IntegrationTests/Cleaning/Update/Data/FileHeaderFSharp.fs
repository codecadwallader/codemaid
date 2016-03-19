module File1

#r "System.Transactions.dll"
open System
open System.Data
open System.Data.SqlClient

// [snippet:Dynamic operator]
let (?) (reader:SqlDataReader) (name:string) : 'R =
  let typ = typeof<'R>
  if typ.IsGenericType && typ.GetGenericTypeDefinition() = typedefof<option<_>> then
    if reader.[name] = box DBNull.Value then
      (box null) :?> 'R
    else typ.GetMethod("Some").Invoke(null, [| reader.[name] |]) :?> 'R
  else
    reader.[name] :?> 'R
// [/snippet]

// [snippet:Example]
let cstr = "Data Source=.\\SQLExpress;Initial Catalog=Northwind;Integrated Security=True"

let printData() =
  use conn = new SqlConnection(cstr)
  conn.Open()
  use cmd = new SqlCommand("SELECT * FROM [Products]", conn)

  printfn "10 Most Expensive Products\n=========================="
  use reader = cmd.ExecuteReader()
  while reader.Read() do
    let name = reader?ProductName
    let price = defaultArg reader?UnitPrice 0.0M
    printfn "%s (%A)" name price

printData()
// [/snippet]