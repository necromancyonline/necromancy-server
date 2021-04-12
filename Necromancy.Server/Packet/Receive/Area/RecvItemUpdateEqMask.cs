using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdateEqMask : PacketResponse
    {
        private readonly ItemInstance _itemInstance;

        public RecvItemUpdateEqMask(NecClient client, ItemInstance itemInstance)
            : base((ushort)AreaPacketId.recv_item_update_eqmask, ServerType.Area)
        {
            _itemInstance = itemInstance;
            Clients.Add(client);
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_itemInstance.instanceId);
            res.WriteInt32((int)_itemInstance.currentEquipSlot);

            res.WriteInt32(_itemInstance.baseId); //Item Base Model ID
            res.WriteByte(0); //Item Revision.  Calls .\data\item\105\model\EM_10500501_05.nif   (note the 05 at the end if set to 5)
            res.WriteByte(0); /*(byte)(_client.Character.RaceId*10+_client.Character.SexId)*/ //??Race and gender tens place is race 1= human, 2= elf 3=dwarf 4=gnome 5=porkul, ones is gender 1 = male 2 = female
            res.WriteByte(0); //??item version

            res.WriteInt32(_itemInstance.baseId); //testing (Theory, texture file related)
            res.WriteByte(0); //hair
            res.WriteByte(0); //color
            res.WriteByte(0); //face

            res.WriteByte(45); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
            res.WriteByte(30); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
            res.WriteByte(00); // testing (Theory Torso Tex)
            res.WriteByte(0); // testing (Theory Pants Tex)
            res.WriteByte(0); // testing (Theory Hands Tex)
            res.WriteByte(0); // testing (Theory Feet Tex)
            res.WriteByte(0); //Alternate texture for item model  0 normal : 1 Pink

            res.WriteByte(0); // Load Cape  1 yes 0 No.   Union Flag?
            res.WriteByte(0); // separate in assembly

            return res;
        }
    }
}
