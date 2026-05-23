/// <summary>
/// logic core game: only join max level unlock. can't play the old levels
/// </summary>
public static class LevelController
{
    static LevelModel levelModel => PlayerModelManager.instance.GetPlayerModel<LevelModel>();

    // Lấy level cao nhất đã hoàn thành (chơi xong)
    public static int GetMaxLevelClear()
    {
        // Nếu level đang mở khóa là 1, nghĩa là chưa vượt qua level nào (trả về 0)
        // Ngược lại, level đã clear chính là (level đang mở khóa - 1)
        return levelModel.lLevel.Value <= 1 ? 0 : levelModel.lLevel.Value - 1;
    }

    // Lấy level cao nhất đang được mở khóa
    public static int GetCurrentLevel()
    {
        return levelModel.lLevel.Value;
    }

    /* LƯU Ý: Hàm GetLevelModelItem đã bị bỏ đi vì bạn không còn dùng List chứa Item nữa.
       Nếu sau này bạn cần lưu số sao (stars) hay điểm (score) cho từng level, 
       bạn nên tạo một Dictionary<int, int> riêng để lưu nhé. Còn hiện tại chỉ cần int là đủ!
    */

    // Vượt qua level
    public static void ClearLevel(int level)
    {
        // Here, we can process other values ​​for the level we just cleared. Ex: stars, score, etc.

        int nextLevel = level + 1;

        // Nếu level tiếp theo lớn hơn level mở khóa hiện tại, thì cập nhật biến lLevel
        // .Value thay đổi sẽ tự động kích hoạt UI thay đổi (nhờ R3)
        if (nextLevel > levelModel.lLevel.Value)
        {
            levelModel.lLevel.Value = nextLevel;
        }

        levelModel.Save();
    }

    // Kiểm tra xem một level bất kỳ đã được mở khóa chưa
    public static bool IsLevelUnlocked(int level)
    {
        // Nó được mở khóa nếu nó nhỏ hơn hoặc bằng level cao nhất đang mở
        return level <= levelModel.lLevel.Value;
    }
}