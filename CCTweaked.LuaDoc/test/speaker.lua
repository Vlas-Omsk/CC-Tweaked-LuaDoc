---@meta

---The speaker peirpheral allow your computer to play notes and other sounds.
---
---The speaker can play three kinds of sound, in increasing orders of complexity: -
---`playNote` allows you to play noteblock note. - `playSound` plays any built-in
---Minecraft sound, such as block sounds or mob noises. - `playAudio` can play
---arbitrary audio.
---
---## Recipe <div class="recipe-container"> <mc-recipe
---recipe="computercraft:speaker"></mc-recipe> </div>
---
---@class speakerlib
---@since 1.80pr1
---@source src/main/java/dan200/computercraft/shared/peripheral/speaker/SpeakerPeripheral.java:52
speaker = {}

---Plays a note block note through the speaker.
---
---This takes the name of a note to play, as well as optionally the volume and
---pitch to play the note at.
---
---The pitch argument uses semitones as the unit. This directly maps to the number
---of clicks on a note block. For reference, 0, 12, and 24 map to F#, and 6 and 18
---map to C.
---
---A maximum of 8 notes can be played in a single tick. If this limit is hit, this
---function will return `false`.
---
---### Valid instruments The speaker supports [all of Minecraft's noteblock
---instruments](https://minecraft.fandom.com/wiki/Note_Block#Instruments). These
---are:
---
---`"harp"`, `"basedrum"`, `"snare"`, `"hat"`, `"bass"`, @code "flute"}, `"bell"`,
---`"guitar"`, `"chime"`, `"xylophone"`, `"iron_xylophone"`, `"cow_bell"`,
---`"didgeridoo"`, `"bit"`, `"banjo"` and `"pling"`.
---
---@param instrument string The instrument to use to play this note.
---@param volume? number The volume to play the note at, from 0.0 to 3.0. Defaults to 1.0.
---@param pitch? number The pitch to play the note at in semitones, from 0 to 24. Defaults to 12.
---@return boolean . Whether the note could be played as the limit was reached.
---@throws If the instrument doesn't exist.
---@source src/main/java/dan200/computercraft/shared/peripheral/speaker/SpeakerPeripheral.java:222
function speaker.playNote(instrument, volume, pitch) end

---Plays a Minecraft sound through the speaker.
---
---This takes the [name of a Minecraft
---sound](https://minecraft.fandom.com/wiki/Sounds.json), such as
---`"minecraft:block.note_block.harp"`, as well as an optional volume and pitch.
---
---Only one sound can be played at once. This function will return `false` if
---another sound was started this tick, or if some `playAudio|audio` is still
---playing.
---
---@param name string The name of the sound to play.
---@param volume? number The volume to play the sound at, from 0.0 to 3.0. Defaults to 1.0.
---@param pitch? number The speed to play the sound at, from 0.5 to 2.0. Defaults to 1.0.
---@return boolean . Whether the sound could be played.
---@throws If the sound name was invalid.
---@usage Play a creeper hiss with the speaker.  ```lua local speaker = peripheral.find("speaker") speaker.playSound("entity.creeper.primed") ```
---@source src/main/java/dan200/computercraft/shared/peripheral/speaker/SpeakerPeripheral.java:271
function speaker.playSound(name, volume, pitch) end

---Attempt to stream some audio data to the speaker.
---
---This accepts a list of audio samples as amplitudes between -128 and 127. These
---are stored in an internal buffer and played back at 48kHz. If this buffer is
---full, this function will return `false`. You should wait for a
---`speaker_audio_empty` event before trying again.
---
---:::note The speaker only buffers a single call to `playAudio` at once. This
---means if you try to play a small number of samples, you'll have a lot of
---stutter. You should try to play as many samples in one call as possible (up to
---128×1024), as this reduces the chances of audio stuttering or halting,
---especially when the server or computer is lagging. :::
---
---`speaker_audio` provides a more complete guide to using speakers
---
---@param audio { [number]: number } A list of amplitudes.
---@param volume? number The volume to play this audio at. If not given, defaults to the previous volumegiven to `playAudio`.
---@return boolean . If there was room to accept this audio data.
---@throws If the audio data is malformed.
---@since 1.100
---@usage Read an audio file, decode it using @{cc.audio.dfpwm}, and play it using the speaker.  ```lua local dfpwm = require("cc.audio.dfpwm") local speaker = peripheral.find("speaker")  local decoder = dfpwm.make_decoder() for chunk in io.lines("data/example.dfpwm", 16 * 1024) do local buffer = decoder(chunk)  while not speaker.playAudio(buffer) do os.pullEvent("speaker_audio_empty") end end ```
---@see cc.audio.dfpwm Provides utilities for decoding DFPWM audio files into a format which can be played bythe speaker.
---@see speaker_audio For a more complete introduction to the @{playAudio} function.
---@source src/main/java/dan200/computercraft/shared/peripheral/speaker/SpeakerPeripheral.java:340
function speaker.playAudio(audio, volume) end

---Stop all audio being played by this speaker.
---
---This clears any audio that `playAudio` had queued and stops the latest sound
---played by `playSound`.
---
---@since 1.100
---@source src/main/java/dan200/computercraft/shared/peripheral/speaker/SpeakerPeripheral.java:369
function speaker.stop() end

