using CCTweaked.LuaDoc.Entities;

namespace CCTweaked.LuaDoc;

// public sealed class EntityWriter : IDisposable
// {
//     private const int _threashold = 80;
//     private const string _commentPrefix = "---";
//     private readonly StreamWriter _writer;
//     private string _moduleName;

//     public EntityWriter(string path)
//     {
//         _writer = new StreamWriter(path);
//     }

//     public void Write(Entity[] entities)
//     {
//         var module = entities.OfType<Module>().First();
//         _moduleName = module.Name;
//         WriteModuleEntity(module);

//         foreach (var entity in entities)
//         {
//             if (entity == null)
//                 continue;

//             if (entity is Function function)
//                 WriteFunctionEntity(function);
//             if (entity is Other other)
//                 WriteOtherEntity(other);
//         }
//     }

//     private void WriteOtherEntity(Other other)
//     {
//         WriteDescription(other);

//         WriteTags(other);
//         foreach (var line in other.Data)
//             _writer.WriteLine(line);
//         _writer.WriteLine();
//     }

//     private void WriteModuleEntity(Module module)
//     {
//         WriteCommentedLine("@meta");
//         _writer.WriteLine();

//         WriteDescription(module);

//         WriteCommentedLine($"@class {module.Name}lib");
//         WriteTags(module);
//         _writer.WriteLine($"{module.Name} = {{}}");
//         _writer.WriteLine();
//     }

//     private void WriteFunctionEntity(Function function)
//     {
//         WriteDescription(function);

//         if (function.Overloads.Length > 0)
//         {
//             foreach (var param in function.Overloads[0].Params)
//             {
//                 var line = $"@param {param.Name}";

//                 if (param.Optional)
//                     line += '?';

//                 WriteCommentedLine($"{line} {param.Type} {param.Description}");
//             }

//             foreach (var @return in function.Overloads[0].Returns)
//             {
//                 var name = @return.Name;

//                 if (string.IsNullOrEmpty(name))
//                     name = ".";

//                 WriteCommentedLine($"@return {@return.Type} {name} {@return.Description}");
//             }
//         }
//         WriteTags(function);
//         if (function.Name.Contains('.'))
//             _writer.Write($"function {function.Name}(");
//         else
//             _writer.Write($"function {_moduleName}.{function.Name}(");
//         if (function.Overloads.Length > 0)
//             _writer.Write(string.Join(", ", function.Overloads[0].Params.Select(x => x.Name)));
//         _writer.WriteLine(") end");
//         _writer.WriteLine();
//     }

//     private void WriteDescription(Entity entity)
//     {
//         if (entity.Description != null)
//         {
//             WriteCommentedText(entity.Description);
//             WriteCommentedLine(null);
//         }
//     }

//     // FIXME
//     private void WriteTags(Entity entity)
//     {
//         // foreach (var tags in entity.Tags)
//         // {
//         //     foreach (var tag in tags.Value)
//         //     {
//         //         var line = '@' + tags.Key;

//         //         if (tag.Params.Length > 0)
//         //             line += string.Join(",", tag.Params.Select(x =>
//         //             {
//         //                 var param = x.Key;

//         //                 if (!string.IsNullOrEmpty(x.Value))
//         //                     param += "=" + x.Value;

//         //                 return param;
//         //             }));

//         //         if (!string.IsNullOrEmpty(tag.Data))
//         //             line += " " + tag.Data;

//         //         WriteCommentedLine(line);
//         //     }
//         // }
//     }

//     private void WriteCommentedText(string text)
//     {
//         var lines = text.Split(Environment.NewLine);

//         foreach (var currentLine in lines)
//         {
//             var words = currentLine.Split(' ');
//             string line = null;

//             foreach (var word in words)
//             {
//                 if ((line?.Length ?? 0) + word.Length + 1 > _threashold)
//                 {
//                     WriteCommentedLine(line);
//                     line = word;
//                 }
//                 else
//                 {
//                     if (line == null)
//                         line = word;
//                     else
//                         line += ' ' + word;
//                 }
//             }

//             WriteCommentedLine(line);
//         }
//     }

//     private void WriteCommentedLine(string line)
//     {
//         _writer.Write(_commentPrefix);
//         if (line != null)
//             _writer.Write(line.Replace(Environment.NewLine, " "));
//         _writer.WriteLine();
//     }

//     public void Dispose()
//     {
//         _writer.Dispose();
//     }
// }
