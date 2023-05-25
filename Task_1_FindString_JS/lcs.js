const findLongestCommonSubstring = (strings) => {
  if (strings.length === 0) {
    return '';
  }
  let longestCommonSubstring = '';
 let shortestString = strings[0];
for (let i = 1; i < strings.length; i++) {
  if (strings[i].length < shortestString.length) {
    shortestString = strings[i];
  }
}  
  for (let i = 0; i < shortestString.length; i++) {
    for (let j = i + 1; j <= shortestString.length; j++) {
      const substring = shortestString.substring(i, j);
      let isCommonSubstring = true;
      for (let k = 0; k < strings.length; k++) {
        if (!strings[k].includes(substring)) {
          isCommonSubstring = false;
          break;
        }
      }
      if (isCommonSubstring && substring.length >= longestCommonSubstring.length) {
        longestCommonSubstring = substring;
      }
    }
  }
  return longestCommonSubstring;
};
const inputStrings = process.argv.slice(2);
const longestCommonSubstring = findLongestCommonSubstring(inputStrings);
console.log(longestCommonSubstring);
