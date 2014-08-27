using System.Runtime.Serialization;

namespace FigLeaf.Core
{
	[DataContract(Namespace = Settings.Namespace)]
	public class DirPair
	{
		[DataMember]
		public string Source { get; set; }
		[DataMember]
		public string Target { get; set; }

		public DirPair(string source, string target)
		{
			Source = source;
			Target = target;
		}
	}
}
