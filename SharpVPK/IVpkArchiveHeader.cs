namespace SharpVPK;

interface IVpkArchiveHeader
{
    uint Signature { get; set; }
    uint Version { get; set; }
    uint TreeLength { get; set; }

    bool Verify();
    uint CalculateDataOffset();
}
