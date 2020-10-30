using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuEditor.Utils;

namespace MuEditor
{
    class CharacterQuestModel
    {
        public CharacterQuestType questType { get; }

        public CharacterQuestModel(CharacterQuestType type)
        {
            this.questType = type;
        }

        public CharacterQuestModel(byte[] value)
        {
            this.questType = GetCharacterQuestType(value);
        }

        //       v-2 prof   v-3 prof      v-4 prof
        // [1  2 3] [4   5  6] [7  8  9  10]
        // 00 00 00 00   00 00 00 00   00 00  = 20 bites = 2.5 bytes
        // 0      5 6       11 12         19

        public static CharacterQuestType GetCharacterQuestType(byte[] value)
        {
            var bits = new List<byte>();
            bits.AddRange(value[0].ReadBits(0, 8));
            bits.AddRange(value[1].ReadBits(0, 8));
            bits.AddRange(value[2].ReadBits(0, 4));


            if ((bits[0] == 1 && bits[1] == 1)
                || (bits[2] == 1 && bits[3] == 1)
                || (bits[4] == 1 && bits[5] == 1))
            {
                return CharacterQuestType.NO_QUEST;
            }

            if ((bits[6] == 1 && bits[7] == 1)
                || (bits[8] == 1 && bits[9] == 1)
                || (bits[10] == 1 && bits[11] == 1))
            {
                return CharacterQuestType.COMPLETE_2;
            }

            if ((bits[12] == 1 && bits[13] == 1)
                || (bits[14] == 1 && bits[15] == 1)
                || (bits[16] == 1 && bits[17] == 1)
                || (bits[18] == 1 && bits[19] == 1))
            {
                return CharacterQuestType.COMPLETE_3;
            }

            return CharacterQuestType.COMPLETE_4;
        }

        public void UpdateTypeIntoVariable(ref byte[] value)
        {
            switch (questType)
            {
                case CharacterQuestType.NO_QUEST:
                    value[0] = 0xFF;
                    value[1] = 0xFF;
                    value[2].WriteBits(1, 0, 4);
                    break;
                case CharacterQuestType.COMPLETE_2:
                    value[0].WriteBits(new byte[] {0, 1, 0, 1, 0, 1, 1, 1}, 0);
                    value[1] = 0xFF;
                    value[2].WriteBits(1, 0, 4);
                    break;
                case CharacterQuestType.COMPLETE_3:
                    value[0].WriteBits(new byte[] {0, 1, 0, 1, 0, 1, 0, 1}, 0);
                    value[1].WriteBits(new byte[] {0, 1, 0, 1, 1, 1, 1, 1}, 0);
                    value[2].WriteBits(1, 0, 4);
                    break;
                case CharacterQuestType.COMPLETE_4:
                    value[0].WriteBits(new byte[] {0, 1, 0, 1, 0, 1, 0, 1}, 0);
                    value[1].WriteBits(new byte[] {0, 1, 0, 1, 0, 1, 0, 1}, 0);
                    value[2].WriteBits(new byte[] {0, 1, 0, 1}, 0);
                    break;
            }
        }

        public override string ToString()
        {
            return questType == CharacterQuestType.COMPLETE_2
                ? "quest for 2-nd profession"
                : questType == CharacterQuestType.COMPLETE_3
                    ? "quest for 3-rd profession"
                    : questType == CharacterQuestType.COMPLETE_4
                        ? "quest for 4-th profession"
                        : "No quest";
        }

        public enum CharacterQuestType
        {
            NO_QUEST,
            COMPLETE_2,
            COMPLETE_3,
            COMPLETE_4
        }
    }
}