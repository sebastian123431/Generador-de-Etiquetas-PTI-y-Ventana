namespace WindowsFormsApplication1.Data
{
    public class Item
    {
        public string Name { get; set; }
        public int Value { get; set; }

        // =========================================================
        // Compatibilidad
        // ---------------------------------------------------------
        // En el proyecto existen pantallas que esperan propiedades
        // "Dato" e "Id" (DisplayMember/ValueMember), mientras que
        // otros formularios usan "Name"/"Value".
        // Estas propiedades son alias para no romper código existente.
        // =========================================================
        public string Dato
        {
            get => Name;
            set => Name = value;
        }

        public int Id
        {
            get => Value;
            set => Value = value;
        }

        public Item(string name, int value)
        {
            Name = name;
            Value = value;
        }

        // Constructor vacío para binding/serialización si se requiere
        public Item() { }

        public override string ToString()
        {
            return Name;
        }
    }
}