using System.Collections.Generic;

namespace Beepify.MIDI
{
    public static class Events
    {
        // Credits: https://github.com/robbie-cao/midi2score/blob/master/midi.h
        // Midi events
        public enum Midi
        {
            MIDI_EVENT_NOTE_OFF         = 0x08,
            MIDI_EVENT_NOTE_ON          = 0x09,
            MIDI_EVENT_AFTER_TOUCH      = 0x0A,
            MIDI_EVENT_CONTROL_CHANGE   = 0x0B,
            MIDI_EVENT_PROGRAM_CHANGE   = 0x0C,
            MIDI_EVENT_CHANNEL_PRESSURE = 0x0D,
            MIDI_EVENT_PITCH_WHEEL      = 0x0E,
            CUSTOM_EVENT_SKIP_NOTE      = 0x100
        }

        // Meta events
        public enum Meta
        {
            SEQUENCE_NUM         = 0x00,
            TEXT_EVNT            = 0x01,
            COPYRIGHT_NOTICE     = 0x02,
            SEQUENCE_NAME        = 0x03,
            INSTRUMENT_NAME      = 0x04,
            LYRICS               = 0x05,
            MARKER               = 0x06,
            CUE_POINT            = 0x07,
            CHANNEL_PREFIX       = 0x20,
            END_TRACK            = 0x2F,
            TEMPO_CHANGE         = 0x51,
            SMPTE_OFFSET         = 0x54,
            TIME_SIGNATURE       = 0x58,
            KEY_SIGNATURE        = 0x59,
            SEQUENCER_SPECIFIC   = 0x7F
        };

        // Control Numbers for 0xBn
        public enum ControllerType
        {
            /* Coarse Control */
            BANK_SELECT          = 0x00,
            MODULATION_WHEEL     = 0x01,
            BREATH               = 0x02,
            FOOT_PEDAL           = 0x04,
            PORTAMENTO_TIME      = 0x05,
            DATA_ENTRY           = 0x06,
            VOLUME               = 0x07,
            BALANCE              = 0x08,
            PAN_POSITION         = 0x0A,
            EXPRESSION           = 0x0B,
            EFFECT_1             = 0x0C,
            EFFECT_2             = 0x0D,
            GENERAL_1            = 0x10,
            GENERAL_2            = 0x11,
            GENERAL_3            = 0x12,
            GENERAL_4            = 0x13,

            /* Fine Control */
            // 0x20 to 0x2D: Same Control as 0x00-0x0D but with fine params

            /* Pedal On/Off Control */
            HOLD_PEDAL           = 0x40,
            PORTAMENTO           = 0x41,
            SOSTENUTO_PEDAL      = 0x42,
            SOFT_PEDAL           = 0x43,
            LEGATO_PEDAL         = 0x44,
            HOLD_2_PEDAL         = 0x45,

            /* Sound Control */
            SOUND_VARIATION      = 0x46,
            SOUND_TIMBRE         = 0x47,
            SOUND_RELEASE_TIME   = 0x48,
            SOUND_ATTACK_TIME    = 0x49,
            SOUND_BRIGHTNESS     = 0x4A,
            SOUND_CONTROL_6      = 0x4B,
            SOUND_CONTROL_7      = 0x4C,
            SOUND_CONTROL_8      = 0x4D,
            SOUND_CONTROL_9      = 0x4E,
            SOUND_CONTROL_10     = 0x4F,

            /* Button Control */
            GENERAL_BUTTON_1     = 0x50,
            GENERAL_BUTTON_2     = 0x51,
            GENERAL_BUTTON_3     = 0x52,
            GENERAL_BUTTON_4     = 0x5e,

            /* Level Control */
            EFFECTS_LEVEL        = 0x5B,
            TREMULO_LEVEL        = 0x5C,
            CHORUS_LEVEL         = 0x5D,
            CELESTE_LEVEL        = 0x5E,
            PHASER_LEVEL         = 0x5F,

            DATA_BUTTON_INC      = 0x60,
            DATA_BUTTON_DEC      = 0x61,
            NON_REG_PARAM_FINE   = 0x62,
            NON_REG_PARAM_COARSE = 0x63,
            REG_PARAM_FINE       = 0x64,
            REG_PARAM_COARSE     = 0x65,

            ALL_SOUND_OFF        = 0x78,
            ALL_CONTROLLERS_OFF  = 0x79,
            LOCAL_KEYBOARD       = 0x7A,
            ALL_NOTES_OFF        = 0x7B,
            OMNI_MODE_OFF        = 0x7C,
            OMNI_MODE_ON         = 0x7D,
            MONO_OPERATION       = 0x7E,
            POLY_OPERATION       = 0x7F,
        }
    }
}
